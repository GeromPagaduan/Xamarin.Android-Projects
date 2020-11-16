using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using FR.Ganfra.Materialspinner;
using DonorTrackerApp.Models;

namespace DonorTrackerApp.Fragments
{
    public class addUserFragment : Android.Support.V4.App.DialogFragment
    {
        MaterialSpinner bloodTypeSpinner;
        List<string> bloodGroupsList;
        ArrayAdapter<string> spinnerAdapter;
        TextInputLayout newUserName, newPhoneNumber, newEmail, newCity, newCountry;
        Button addButton;
        private string selectedBloodType;
        public event EventHandler<DonorDetailsEventArgs> OnDonorRegistered;

        public class DonorDetailsEventArgs : EventArgs
        {
            public Donor Donor {get; set;}
        }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }


        void ConnectViews(View view)
        {
            newUserName = (TextInputLayout)view.FindViewById(Resource.Id.newUserName);
            newPhoneNumber = (TextInputLayout)view.FindViewById(Resource.Id.newPhoneNumber);
            newEmail = (TextInputLayout)view.FindViewById(Resource.Id.newEmail);
            newCity = (TextInputLayout)view.FindViewById(Resource.Id.newCity);
            newCountry = (TextInputLayout)view.FindViewById(Resource.Id.newCountry);
            bloodTypeSpinner = (MaterialSpinner)view.FindViewById(Resource.Id.bloodTypeSpinner);
            addButton = (Button)view.FindViewById(Resource.Id.addButton);

            addButton.Click += AddButton_Click;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string name, phone, email, city, country;
            name = newUserName.EditText.Text;
            phone = newPhoneNumber.EditText.Text;
            email = newEmail.EditText.Text;
            city = newCity.EditText.Text;
            country = newCountry.EditText.Text;

            if (name.Length < 5)
            {
                Toast.MakeText(Activity, "Please provide a valid name.", ToastLength.Short).Show();
                return;
            }
            else if (!email.Contains("@"))
            {
                Toast.MakeText(Activity, "Please provide a valid email address.", ToastLength.Short).Show();
                return;
            }
            else if (phone.Length < 10)
            {
                Toast.MakeText(Activity, "Please provide a valid phone number.", ToastLength.Short).Show();
                return;
            }
            else if (city.Length < 2)
            {
                Toast.MakeText(Activity, "Please provide a valid city.", ToastLength.Short).Show();
                return;
            }
            else if (country.Length < 2)
            {
                Toast.MakeText(Activity, "Please provide a valid country.", ToastLength.Short).Show();
                return;
            }
            else if (selectedBloodType.Length < 2)
            {
                Toast.MakeText(Activity, "Please provide a valid blood type.", ToastLength.Short).Show();
                return;
            }

            Donor newDonor = new Donor();
            newDonor.Fullname = name;
            newDonor.Phone = phone;
            newDonor.Email = email;
            newDonor.City = city;
            newDonor.Country = country;
            newDonor.BloodGroup = selectedBloodType;

            OnDonorRegistered?.Invoke(this, new DonorDetailsEventArgs { Donor = newDonor });
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.addUser, container, false);
            ConnectViews(view);
            SetupSpinner();
            return view;
        }


        // Populates Spinner with Blood Type options
        void SetupSpinner()
        {
            bloodGroupsList = new List<string>();
            bloodGroupsList.Add("A+");
            bloodGroupsList.Add("A-");
            bloodGroupsList.Add("B+");
            bloodGroupsList.Add("B-");
            bloodGroupsList.Add("AB+");
            bloodGroupsList.Add("AB-");
            bloodGroupsList.Add("O+");
            bloodGroupsList.Add("O-");

            spinnerAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, bloodGroupsList);
            spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            bloodTypeSpinner.Adapter = spinnerAdapter;
            bloodTypeSpinner.ItemSelected += BloodTypeSpinner_ItemSelected;
        }

        private void BloodTypeSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if(e.Position != -1)
            {
                selectedBloodType = bloodGroupsList[e.Position];
                Console.WriteLine(selectedBloodType);
            }
        }
    }
}