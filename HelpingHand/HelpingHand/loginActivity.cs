using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Firebase.Xamarin.Auth;
using Firebase.Auth;
using System;
using static Android.Views.View;
using Android.Views;
using Android.Gms.Tasks;
using Android.Support.Design.Widget;
using Firebase;
using XamarinFirebaseAuth;

namespace HelpingHand
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/AppTheme")]
    public class loginActivity : Activity, IOnClickListener, IOnCompleteListener
    {
        Button btnLogin;
        EditText input_email, input_password;
        TextView btnSignUp, btnForgetPassword;
        RelativeLayout login_Activity;
        public static FirebaseApp app;
        Firebase.Auth.FirebaseAuth auth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.login);
            //Init Auth  
            InitFirebaseAuth();
            //Views  
            btnLogin = FindViewById<Button>(Resource.Id.login_btn_login);
            input_email = FindViewById<EditText>(Resource.Id.login_email);
            input_password = FindViewById<EditText>(Resource.Id.login_password);
            btnSignUp = FindViewById<TextView>(Resource.Id.login_btn_sign_up);
            btnForgetPassword = FindViewById<TextView>(Resource.Id.login_btn_forget_password);
            login_Activity = FindViewById<RelativeLayout>(Resource.Id.login);
            btnSignUp.SetOnClickListener(this);
            btnLogin.SetOnClickListener(this);
            btnForgetPassword.SetOnClickListener(this);
        }
        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder()
               .SetApplicationId("AIzaSyDDkjTIE-LQMNCfPOwzR8kX0-IPENxl_xY")
               .SetApiKey("AIzaSyBmuAwrNEgENM40rnjUToHHMraFXQOyuPE")
               .Build();
            if (app == null)
                app = FirebaseApp.InitializeApp(this, options);
            auth = Firebase.Auth.FirebaseAuth.GetInstance(app);
        }
        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.login_btn_forget_password)
            {
                StartActivity(new Android.Content.Intent(this, typeof(ForgetPassword)));
                Finish();
            }
            else
            if (v.Id == Resource.Id.login_btn_sign_up)
            {
                StartActivity(new Android.Content.Intent(this, typeof(SignUp)));
                Finish();
            }
            else
            if (v.Id == Resource.Id.login_btn_login)
            {
                LoginUser(input_email.Text, input_password.Text);
            }
        }
        private void LoginUser(string email, string password)
        {
            auth.SignInWithEmailAndPassword(email, password).AddOnCompleteListener(this);
        }
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                StartActivity(new Android.Content.Intent(this, typeof(DashBoard)));
                Finish();
            }
            else
            {
                Snackbar snackbar = Snackbar.Make(login_Activity, "Login Failed ", Snackbar.LengthShort);
                snackbar.Show();
            }
        }
    }
}