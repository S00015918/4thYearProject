using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HelpingHand.Model;

namespace HelpingHand
{
    public class ListViewAdapter : BaseAdapter
    {
        Activity activity;
        List<Parent> lstParents;
        LayoutInflater inflater;

        public ListViewAdapter(Activity activity, List<Parent> lstAccounts)
        {
            this.activity = activity;
            this.lstParents = lstAccounts;
        }
        public override int Count
        {
            get { return lstParents.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            inflater = (LayoutInflater)activity.BaseContext.GetSystemService(Context.LayoutInflaterService);
            View itemView = inflater.Inflate(Resource.Layout.list_Item, null);
            var txtuser = itemView.FindViewById<TextView>(Resource.Id.list_name);
            var txtemail = itemView.FindViewById<TextView>(Resource.Id.list_email);
            if (lstParents.Count > 0)
            {
                txtuser.Text = lstParents[position].name;
                txtemail.Text = lstParents[position].email;
            }
            return itemView;
        }
    }
}