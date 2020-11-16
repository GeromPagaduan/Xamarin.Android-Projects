using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace IncomePlanner
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText incomePerHourInput, workHoursInput, taxRateInput, savingsRateInput;
        TextView workSummaryValue, grossIncomeValue, annualTaxValue, annualSavingsValue, spendableIncomeValue;
        Button calculateButton;
        RelativeLayout summary;
        bool inputCalculated = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            ConnectViews();
        }

        void ConnectViews()
        {
            incomePerHourInput = (EditText)FindViewById(Resource.Id.incomePerHourInput);
            workHoursInput = (EditText)FindViewById(Resource.Id.workHoursInput);
            taxRateInput = (EditText)FindViewById(Resource.Id.taxRateInput);
            savingsRateInput = (EditText)FindViewById(Resource.Id.savingsRateInput);

            workSummaryValue = (TextView)FindViewById(Resource.Id.workSummaryValue);
            grossIncomeValue = (TextView)FindViewById(Resource.Id.grossIncomeValue);
            annualTaxValue = (TextView)FindViewById(Resource.Id.annualTaxValue);
            annualSavingsValue = (TextView)FindViewById(Resource.Id.annualSavingsValue);
            spendableIncomeValue = (TextView)FindViewById(Resource.Id.spendableIncomeValue);

            calculateButton = (Button)FindViewById(Resource.Id.calculateButton);
            summary = (RelativeLayout)FindViewById(Resource.Id.summary);

            calculateButton.Click += CalculateButton_Click;
        }


        void ClearInput()
        {
            incomePerHourInput.Text = "";
            workHoursInput.Text = "";
            taxRateInput.Text = "";
            savingsRateInput.Text = "";
            summary.Visibility = Android.Views.ViewStates.Invisible;
        }


        private void CalculateButton_Click(object sender, System.EventArgs e)
        {
            if (inputCalculated)
            {
                inputCalculated = false;
                calculateButton.Text = "Calculate";
                ClearInput();
                return;
            }
            // Read inputs from user and store into variable
            double incomePerHour = double.Parse(incomePerHourInput.Text);
            double workHours = double.Parse(workHoursInput.Text);
            double taxRate = double.Parse(taxRateInput.Text);
            double savingsRate = double.Parse(savingsRateInput.Text);

            double annualWorkHourTotal = workHours * 250; // Assumes working 5 days a week, with 2 weeks off: (5 * (52 - 2))
            double annualIncome = incomePerHour * annualWorkHourTotal;
            double taxPayable = (taxRate / 100) * annualIncome;
            double annualSavings = (savingsRate / 100) * annualIncome;
            double spendableIncome = annualIncome - annualSavings - taxPayable;

            // Display result of calculations
            grossIncomeValue.Text = annualIncome.ToString("#,##") + " Dollars (US)";
            workSummaryValue.Text = annualWorkHourTotal.ToString("#,##") + " Hours";
            annualTaxValue.Text = taxPayable.ToString("#,##") + " Dollars (US)";
            annualSavingsValue.Text = annualSavings.ToString("#,##") + " Dollars (US)";
            spendableIncomeValue.Text = spendableIncome.ToString("#,##") + " Dollars (US)";

            summary.Visibility = Android.Views.ViewStates.Visible;
            inputCalculated = true;
            calculateButton.Text = "Clear";
        }
    }
}