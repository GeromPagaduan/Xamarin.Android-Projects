using System;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Support.V7.Widget;
using DonorTrackerApp.Adapters;
using System.Collections.Generic;
using DonorTrackerApp.Models;
using Android.Content;
using Android.Support.Design.Widget;
using DonorTrackerApp.Fragments;
using Newtonsoft.Json;

namespace DonorTrackerApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]

    public class MainActivity : AppCompatActivity
    {
        RecyclerView donorRecyclerView;
        TextView emptyDonorView;
        UserAdapter donorsAdapter;
        List<Donor> listOfDonors = new List<Donor>();
        addUserFragment addUserFragment;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("donors", FileCreationMode.Private);
        ISharedPreferencesEditor editor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            SupportActionBar.Title = "Blood Donors";
            emptyDonorView = (TextView)FindViewById(Resource.Id.emptyDonorView);

            donorRecyclerView = (RecyclerView)FindViewById(Resource.Id.donorRecyclerView);
            FloatingActionButton fab = (FloatingActionButton)FindViewById(Resource.Id.fab);
            fab.Click += Fab_Click;
            // CreateData();  // Replace with RetrieveData
            RetrieveData();
            if(listOfDonors.Count > 0)
            {
                SetupRecyclerView();
            }
            else
            {
                emptyDonorView.Visibility = Android.Views.ViewStates.Visible;  // If empty list, make placeholder textview visible
            }
            editor = pref.Edit();
        }


        private void Fab_Click(object sender, System.EventArgs e)
        {
            addUserFragment = new addUserFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            addUserFragment.Show(trans, "new donor");
            addUserFragment.OnDonorRegistered += AddUserFragment_OnDonorRegistered;
        }

        private void AddUserFragment_OnDonorRegistered(object sender, addUserFragment.DonorDetailsEventArgs e)
        {
            if(addUserFragment != null)
            {
                addUserFragment.Dismiss();
                addUserFragment = null;
            }
            if(listOfDonors.Count > 0)
            {
                listOfDonors.Insert(0, e.Donor);
                donorsAdapter.NotifyItemInserted(0);

                string jsonString = JsonConvert.SerializeObject(listOfDonors);  // Serializes list we passed in into JSON string (using JSON Newton Nuget)
                editor.PutString("donors", jsonString);
                editor.Apply();  // Saves donors to shared preferences
            }
            else
            {
                // If we try to add to a recycler view while recycler view has not been set up, error will occur and this catches it
                listOfDonors.Add(e.Donor);
                string jsonString = JsonConvert.SerializeObject(listOfDonors);  // Serializes list we passed in into JSON string (using JSON Newton Nuget)
                editor.PutString("donors", jsonString);
                editor.Apply();

                SetupRecyclerView();
            }
            
        }

        void CreateData()
        {
            listOfDonors = new List<Donor>();
            listOfDonors.Add(new Donor { BloodGroup = "O+", City = "Littleton", Country = "USA", Email = "gpagaduan4@gmail.com", Fullname = "Gerom Angelo Pagaduan", Phone = "720-592-8937" });
            listOfDonors.Add(new Donor { BloodGroup = "A-", City = "Littleton", Country = "USA", Email = "gpagaduan4@gmail.com", Fullname = "Jerry Angelus Paduan", Phone = "720-592-8938" });
            listOfDonors.Add(new Donor { BloodGroup = "AB-", City = "Littleton", Country = "USA", Email = "gpagaduan4@gmail.com", Fullname = "Rom Angel Pagad", Phone = "720-592-8939" });
            listOfDonors.Add(new Donor { BloodGroup = "B+", City = "Littleton", Country = "USA", Email = "gpagaduan4@gmail.com", Fullname = "Gero Gelo Gaduan", Phone = "720-592-8930" });
        }


        // Retrieve donors from shared preferences
        void RetrieveData()
        {
            string json = pref.GetString("donors", "");
            if (!string.IsNullOrEmpty(json))
            {
                listOfDonors = JsonConvert.DeserializeObject<List<Donor>>(json);  // If not empty, deserialize the json string and make a list of Donors
            }
        }


        void SetupRecyclerView()
        {
            donorRecyclerView.SetLayoutManager(new LinearLayoutManager(donorRecyclerView.Context));
            donorsAdapter = new UserAdapter(listOfDonors);
            donorsAdapter.ItemClick += DonorsAdapter_ItemClick;
            donorsAdapter.CallClick += DonorsAdapter_CallClick;
            donorsAdapter.MailClick += DonorsAdapter_MailClick;
            donorsAdapter.DeleteClick += DonorsAdapter_DeleteClick;
            donorRecyclerView.SetAdapter(donorsAdapter);

            emptyDonorView.Visibility = Android.Views.ViewStates.Invisible;  // Make placeholder textview invisible since we now have an element in a list
        }

        private void DonorsAdapter_DeleteClick(object sender, UserAdapterClickEventArgs e)
        {
            var donor = listOfDonors[e.Position];
            Android.Support.V7.App.AlertDialog.Builder DeleteAlert = new Android.Support.V7.App.AlertDialog.Builder(this);
            DeleteAlert.SetMessage("Are you sure?");
            DeleteAlert.SetTitle("Delete Donor");
            

            DeleteAlert.SetPositiveButton("Delete", (alert, args) =>
            {
                listOfDonors.RemoveAt(e.Position);
                donorsAdapter.NotifyItemRemoved(e.Position);  // donorsAdapter.NotifyDataSetChanged(); <- not using because refreshes whole view. Potential loss of data

                // Update shared preferences when a row is deleted from list
                string jsonString = JsonConvert.SerializeObject(listOfDonors);  // Serializes list we passed in into JSON string (using JSON Newton Nuget)
                editor.PutString("donors", jsonString);
                editor.Apply();  // Saves donors to shared preferences
            });

            DeleteAlert.SetNegativeButton("Cancel", (alert, args) =>
            {
                DeleteAlert.Dispose();
            });

            DeleteAlert.Show();
        }

        private void DonorsAdapter_MailClick(object sender, UserAdapterClickEventArgs e)
        {
            var donor = listOfDonors[e.Position];
            Android.Support.V7.App.AlertDialog.Builder MailAlert = new Android.Support.V7.App.AlertDialog.Builder(this);
            MailAlert.SetMessage("Send Mail to " + donor.Fullname);

            MailAlert.SetPositiveButton("Send", (alert, args) =>
            {
                Intent intent = new Intent();
                intent.SetType("plain/text");
                intent.SetAction(Intent.ActionSend);
                intent.PutExtra(Intent.ExtraEmail, new string[] { donor.Email });
                intent.PutExtra(Intent.ExtraSubject, new string[] { "Enter your message here." });
                StartActivity(intent);
            });

            MailAlert.SetNegativeButton("Cancel", (alert, args) =>
            {
                MailAlert.Dispose();
            });

            MailAlert.Show();

        }


        // What happens when you click "Call"
        private void DonorsAdapter_CallClick(object sender, UserAdapterClickEventArgs e)
        {
            var donor = listOfDonors[e.Position];

            Android.Support.V7.App.AlertDialog.Builder CallAlert = new Android.Support.V7.App.AlertDialog.Builder(this);
            CallAlert.SetMessage("Call " + donor.Fullname);

            CallAlert.SetPositiveButton("Call", (alert, args) =>
            {
                var uri = Android.Net.Uri.Parse("tel:" + donor.Phone);
                var intent = new Intent(Intent.ActionDial, uri);
                StartActivity(intent);
            });

            CallAlert.SetNegativeButton("Cancel", (alert, args) =>
            {
                CallAlert.Dispose();
            });

            CallAlert.Show();
        }

        private void DonorsAdapter_ItemClick(object sender, UserAdapterClickEventArgs e)
        {
            Toast.MakeText(this, "Row was clicked", ToastLength.Short).Show();
        }
    }
}