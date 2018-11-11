using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using HelpingHand.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Views;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using System;

namespace HelpingHand
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class MainActivity : AppCompatActivity
    {
        private EditText input_name, input_email, input_surname, input_phone, input_address, input_city;
        private ListView list_data;
        private ProgressBar circular_progress;

        private List<Parent> list_parents = new List<Parent>();
        private ListViewAdapter adapter;
        private Parent selectedParent;


        private const string FirebaseURL = "https://th-year-project-37928.firebaseio.com/";

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //Add Toolbar

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Helping Hand";
            SetSupportActionBar(toolbar);

            //View 

            circular_progress = FindViewById<ProgressBar>(Resource.Id.circularProgress);
            input_name = FindViewById<EditText>(Resource.Id.name);
            input_surname = FindViewById<EditText>(Resource.Id.surname);
            input_email = FindViewById<EditText>(Resource.Id.email);
            input_phone = FindViewById<EditText>(Resource.Id.phone);
            input_address = FindViewById<EditText>(Resource.Id.address);
            input_city = FindViewById<EditText>(Resource.Id.city);
            list_data = FindViewById<ListView>(Resource.Id.list_data);
            list_data.ItemClick += (s, e) =>
            {
                Parent account = list_parents[e.Position];
                selectedParent = account;
                input_name.Text = account.name;
                input_email.Text = account.email;
            };

            await LoadData();
        }

        private async Task LoadData()
        {
            circular_progress.Visibility = ViewStates.Visible;
            list_data.Visibility = ViewStates.Invisible;

            var firebase = new FirebaseClient(FirebaseURL);
            var items = await firebase
                .Child("parents")
                .OnceAsync<Parent>();

            list_parents.Clear();
            adapter = null;
            foreach (var item in items)
            {
                Parent par = new Parent
                {
                    id = item.Key,
                    name = item.Object.name,
                    email = item.Object.email
                };

                list_parents.Add(par);
            }
            adapter = new ListViewAdapter(this, list_parents);
            adapter.NotifyDataSetChanged();
            list_data.Adapter = adapter;

            circular_progress.Visibility = ViewStates.Invisible;
            list_data.Visibility = ViewStates.Visible;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.menu_add)
            {
                CreateUser();
            }
            else if (id == Resource.Id.menu_save) //Update
            {
                UpdateUser(selectedParent.id, input_name.Text, input_email.Text);
            }
            else if (id == Resource.Id.menu_delete) //Delete
            {
                DeleteUser(selectedParent.id);
            }
            return base.OnOptionsItemSelected(item);
        }

        private async void DeleteUser(string id)
        {
            var firebase = new FirebaseClient(FirebaseURL);
            await firebase.Child("parents").Child(id).DeleteAsync();
            await LoadData();
        }

        private async void UpdateUser(string id, string name, string email)
        {
            var firebase = new FirebaseClient(FirebaseURL);
            await firebase.Child("parents").Child(id).Child("parents").PutAsync(name);
            await firebase.Child("parents").Child(id).Child("parents").PutAsync(email);

            await LoadData();
        }

        private async void CreateUser()
        {
            var spinner = FindViewById<Spinner>(Resource.Id.spinnerCount);
            spinner.ItemSelected += (s, e) =>
            {
                string firstItem = spinner.SelectedItem.ToString();
                if (firstItem.Equals(spinner.SelectedItem.ToString()))
                {
                    // To do when first item is selected
                }
                else
                {
                    Toast.MakeText(this, "You have selected " + e.Parent.GetItemIdAtPosition(e.Position).ToString(),
                        ToastLength.Short).Show();
                }
            };


            int input_childCount = int.Parse(spinner.SelectedItem.ToString());

            Parent parent = new Parent();
            //parent.id = String.Empty;
            parent.name = input_name.Text;
            parent.surname = input_surname.Text;
            parent.email = input_email.Text;
            parent.phone = input_phone.Text;
            parent.city = input_city.Text;
            parent.address = input_address.Text;
            parent.noOfKids = Convert.ToInt32(input_childCount);

            var firebase = new FirebaseClient(FirebaseURL);
            //Add Item
            var item = await firebase.Child("parent").PostAsync<Parent>(parent);
            await LoadData();
        }
    }
}