using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Java.Util;
using Android.Icu.Util;

namespace LuckyNumber
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        // Instantiates elements to interact with design
        SeekBar rangeSetter;
        TextView resultNumber, luckyRange;
        Button rollButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main); // Sets view to "activity_main.xml"
            // Uses our set instances to match it to the same elements in our design (by id)
            rangeSetter = (SeekBar)FindViewById(Resource.Id.rangeSetter);
            luckyRange = (TextView)FindViewById(Resource.Id.luckyRange);
            resultNumber = (TextView)FindViewById(Resource.Id.resultNumber);
            rollButton = (Button)FindViewById(Resource.Id.rollButton);
            SupportActionBar.Title = "Lucky Number Generator"; // Sets title
            // Click event handler added to button
            rollButton.Click += RollButton_Click;
            // Show SeekBar value
            rangeSetter.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    luckyRange.Text = string.Format("Your range is 1 to {0}", e.Progress);
                }
            };
        }

        // The click button event handler
        private void RollButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                Random random = new Random(); // Generate random number
                int luckyNumber = random.NextInt(rangeSetter.Progress); // Sets range of possible number's range from 1 to MAX range
                resultNumber.Text = luckyNumber.ToString(); // Displays generated lucky number
            }
            catch (System.Exception)
            {
                resultNumber.Text = "0"; // Returns a 0 if slider is moved to maximum left
            }
        }
    }
}