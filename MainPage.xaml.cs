using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Calculator
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            this.Result = "0";
            BindingContext = this;

        }
        string result;
        public string Result
        {
            get => this.result;
            set
            {
                if (this.result != value) { 
                    this.result = value;
                    OnPropertyChanged(nameof(this.Result));
                }
            }
        }
        private string Left, op, Right;
        public string Op
        {
            get => this.op;
            set
            {
                if (this.op != value)
                {
                    this.op = value;
                    OnPropertyChanged(nameof(this.Op));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private void NumberClicked(object sender, EventArgs e)
        {
            if (Char.IsDigit(((Button)sender).Text.ToCharArray()[0]) || ((Button)sender).Text == ".")
            {
                NumberAssigner(((Button)sender).Text, Binary: true);
                ResultAssigner();
                if (this.Result != "0") this.Clear.Text = "C";
            }
        }
        private void OpClicked(object sender, EventArgs e)
        {
            switch (((Button)sender).Text)
            {
                case "C":
                    this.Result = "0";
                    if (this.Op != null) this.Right = null; else this.Left = null;
                    this.Clear.Text = "AC";
                    break;
                case "AC":
                    this.Result = "0";
                    if (this.Op != null) this.Left = null;
                    this.Op = null;
                    break;
                case "+/-":
                    this.Result = "-" +(Double.Parse(this.Result)).ToString();
                    NumberAssigner(this.Result);
                    ResultAssigner(); 
                    break;
                case "%":
                    this.Result = (Double.Parse(this.Result) / 100).ToString();
                    NumberAssigner(this.Result);
                    ResultAssigner();
                    break;
                case "÷":
                    this.Op = "/";
                    break;
                case "x":
                    this.Op = "*";
                    break;
                case "=":
                    try
                    {
                        if (this.Result != "Error" && this.Result != "0")
                        {
                            this.Result = (this.Result.StartsWith("-") ?
                                ((this.Op != null) ?
                                new System.Data.DataTable().Compute("-" + this.Left + this.Op + this.Right, String.Empty).ToString() :
                                "-" + new System.Data.DataTable().Compute("-" + this.Left, String.Empty).ToString()) :
                                ((this.Op != null) ? new System.Data.DataTable().Compute(this.Left + this.Op + this.Right, String.Empty).ToString() : new System.Data.DataTable().Compute(this.Left, String.Empty).ToString()));
                            this.Right = this.Op = null;
                            this.Left = this.Result;
                            this.ResultAssigner();
                        }
                    }
                    catch { this.Result = "Error"; this.Left = this.Op = null; }
                    break;
                default:
                    this.Op = ((Button)sender).Text;
                    break;
            }

        }
        private void ResultAssigner()
        {
            if (this.Op != null) { this.Result = this.Right; }
            else { this.Result = this.Left; }
        }
        private void NumberAssigner(string Number, bool Binary = false)
        {
            if (this.Op != null) { if (Binary == true) this.Right += Number; else this.Right = Number; }
            else { if (Binary == true) this.Left += Number; else this.Left = Number; }
        }
    }
}