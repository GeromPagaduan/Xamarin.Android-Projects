using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using DonorTrackerApp.Models;
using Java.Security;

namespace DonorTrackerApp.Adapters
{
    class UserAdapter : RecyclerView.Adapter
    {
        public event EventHandler<UserAdapterClickEventArgs> ItemClick;
        public event EventHandler<UserAdapterClickEventArgs> ItemLongClick;
        public event EventHandler<UserAdapterClickEventArgs> CallClick;
        public event EventHandler<UserAdapterClickEventArgs> MailClick;
        public event EventHandler<UserAdapterClickEventArgs> DeleteClick;

        List<Donor> DonorsList;

        public UserAdapter(List<Donor> data)
        {
            DonorsList = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.donor_row, parent, false);

            var vh = new UserAdapterViewHolder(itemView, OnClick, OnLongClick, OnCallClick, OnMailClick, OnDeleteClick);
            return vh;
        }


        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var donor = DonorsList[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as UserAdapterViewHolder;
            holder.donorText.Text = donor.Fullname;
            holder.donorLocation.Text = donor.City + ", " + donor.Country;
            
            // Assign images corresponding to blood type
            if (donor.BloodGroup == "A+")
            {
                holder.donorImage.SetImageResource(Resource.Drawable.a_positive);
            }
            else if(donor.BloodGroup == "A-"){
                holder.donorImage.SetImageResource(Resource.Drawable.a_negative);
            }
            else if (donor.BloodGroup == "B+")
            {
                holder.donorImage.SetImageResource(Resource.Drawable.b_positive);
            }
            else if (donor.BloodGroup == "B-")
            {
                holder.donorImage.SetImageResource(Resource.Drawable.b_negative);
            }
            else if (donor.BloodGroup == "AB+")
            {
                holder.donorImage.SetImageResource(Resource.Drawable.ab_positive);
            }
            else if (donor.BloodGroup == "AB-")
            {
                holder.donorImage.SetImageResource(Resource.Drawable.ab_negative);
            }
            else if (donor.BloodGroup == "O+")
            {
                holder.donorImage.SetImageResource(Resource.Drawable.o_ppositive);
            }
            else if (donor.BloodGroup == "O-")
            {
                holder.donorImage.SetImageResource(Resource.Drawable.o_negative);
            }
        }

        public override int ItemCount => DonorsList.Count;

        void OnClick(UserAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(UserAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

        void OnCallClick(UserAdapterClickEventArgs args) => CallClick?.Invoke(this, args);
        void OnMailClick(UserAdapterClickEventArgs args) => MailClick?.Invoke(this, args);
        void OnDeleteClick(UserAdapterClickEventArgs args) => DeleteClick?.Invoke(this, args);

    }

    public class UserAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public ImageView donorImage;
        public TextView donorText, donorLocation;
        public RelativeLayout callButton, mailButton, deleteButton;


        public UserAdapterViewHolder(View itemView, Action<UserAdapterClickEventArgs> clickListener,
                            Action<UserAdapterClickEventArgs> longClickListener, Action<UserAdapterClickEventArgs> callClickListener,
                            Action<UserAdapterClickEventArgs> mailClickListener, Action<UserAdapterClickEventArgs> deleteClickListener) : base(itemView)
        {
            //TextView = v;
            donorImage = (ImageView)itemView.FindViewById(Resource.Id.donorImage);
            donorText = (TextView)itemView.FindViewById(Resource.Id.donorText);
            donorLocation = (TextView)itemView.FindViewById(Resource.Id.donorLocation);
            callButton = (RelativeLayout)itemView.FindViewById(Resource.Id.callButton);
            mailButton = (RelativeLayout)itemView.FindViewById(Resource.Id.mailButton);
            deleteButton = (RelativeLayout)itemView.FindViewById(Resource.Id.deleteButton);

            itemView.Click += (sender, e) => clickListener(new UserAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new UserAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            callButton.Click += (sender, e) => callClickListener(new UserAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            mailButton.Click += (sender, e) => mailClickListener(new UserAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            deleteButton.Click += (sender, e) => deleteClickListener(new UserAdapterClickEventArgs { View = itemView, Position = AdapterPosition });

        }
    }

    public class UserAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}