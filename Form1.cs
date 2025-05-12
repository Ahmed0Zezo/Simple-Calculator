namespace Calculator_V2
{
    public partial class Calculator : Form
    {

        enum enOperations { eAdd = 1, eSubtract, eMultiply, eDivide };
        enum enModes
        {
            eIdle = 1, eEnteringNumber, eSelectFirstOperation, eChainOperator
                , eResultShown, eError
        };


        public Calculator()
        {
            InitializeComponent();
        }

        bool IsFirstOperation = true;
        double CurrentValue = 0;
        double PreviousValue = 0;
        enModes CurrentMode = enModes.eIdle;
        enOperations CurrentOperation = enOperations.eAdd;


        void AddNumberToLabel(Button btn)
        {
            if (lbResult.Text == "0" || CurrentMode == enModes.eIdle)
            {
                lbResult.Text = btn.Text;

            }
            else
            {
                lbResult.Text += btn.Text;
            }

        }

        double DivideOperation(double Number1, double Number2)
        {
            if (Number2 == 0)
            {
                ActivateErrorMode();

                return 0;
            }

            return Number1 / Number2;

        }

        void Calculation(enOperations Operation)
        {
            switch (Operation)
            {
                case enOperations.eAdd:
                    PreviousValue += CurrentValue;
                    break;
                case enOperations.eSubtract:
                    PreviousValue -= CurrentValue;
                    break;
                case enOperations.eMultiply:
                    PreviousValue *= CurrentValue;
                    break;
                case enOperations.eDivide:
                    PreviousValue = DivideOperation(PreviousValue, CurrentValue);

                    break;
            }
        }
        void ActivateErrorMode()
        {
            CurrentMode = enModes.eError;
            lbResult.Text = "Error";
            btn0.Enabled = false;
            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;
            btn5.Enabled = false;
            btn6.Enabled = false;
            btn7.Enabled = false;
            btn8.Enabled = false;
            btn9.Enabled = false;
            btnPlus.Enabled = false;
            btnMinus.Enabled = false;
            btnMultiply.Enabled = false;
            btnDivide.Enabled = false;
            btnBackSpace.Enabled = false;
            btnDot.Enabled = false;
            btnReverse.Enabled = false;
            btnEqualls.Enabled = false;

        }

        void UpdateCurrentOperation(Button btn)
        {
            if (btn.Tag != null)
                CurrentOperation = (enOperations)Convert.ToInt16(btn.Tag);
            else
            {
                MessageBox.Show("Tag is Null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        void UpdateCurrentValue()
        {
            CurrentValue = Convert.ToDouble(lbResult.Text);
        }

        void OpDuringNumberMode(Button btn)
        {

            if (IsFirstOperation)
            {
                PreviousValue = CurrentValue;
                CurrentValue = 0;
                IsFirstOperation = false;
                CurrentMode = enModes.eSelectFirstOperation;
            }
            else
            {
                Calculation(CurrentOperation);
                if (CurrentMode == enModes.eError)
                {
                    return;
                }
                CurrentMode = enModes.eChainOperator;

            }

            UpdateCurrentOperation(btn);

            lbResult.Text = "0";

        }

        void NumbersAfterEqualMode(Button btn)
        {
            PreviousValue = 0;
            lbResult.Text = btn.Text;
            CurrentMode = enModes.eEnteringNumber;
            CurrentOperation = enOperations.eAdd;
            UpdateCurrentValue();
            IsFirstOperation = true;

        }
        void EqualDuringNumbers()
        {
            if (IsFirstOperation && CurrentMode != enModes.eEnteringNumber)
            {
                ActivateErrorMode();
            }
            else
            {

                Calculation(CurrentOperation);
                if (CurrentMode == enModes.eError)
                {
                    lbResult.Text = "Cannot Divide by Zero";
                    return;
                }
                lbResult.Text = PreviousValue.ToString();
                CurrentMode = enModes.eResultShown;
            }
        }

        void OpAfterEqualMode(Button btn)
        {
            CurrentMode = enModes.eChainOperator;
            lbResult.Text = "0";
            UpdateCurrentValue();
            IsFirstOperation = false;
            UpdateCurrentOperation(btn);
        }
        void BackSpace()
        {
            if (lbResult.Text.Length >= 2)
            {
                if (CurrentMode != enModes.eResultShown)
                {
                    lbResult.Text = lbResult.Text.Remove(lbResult.Text.Length - 1);
                }


            }
            else
            {
                lbResult.Text = "0";
            }
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            BackSpace();
            UpdateCurrentValue();

        }
        private void NumericButtons_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (CurrentMode)
            {
                case enModes.eIdle:
                    CurrentMode = enModes.eEnteringNumber;
                    AddNumberToLabel(btn);
                    UpdateCurrentValue();
                    break;

                case enModes.eEnteringNumber:
                    AddNumberToLabel(btn);
                    UpdateCurrentValue();
                    break;
                case enModes.eSelectFirstOperation:
                    CurrentMode = enModes.eEnteringNumber;
                    AddNumberToLabel(btn);
                    UpdateCurrentValue();
                    break;
                case enModes.eChainOperator:
                    CurrentMode = enModes.eEnteringNumber;
                    AddNumberToLabel(btn);
                    UpdateCurrentValue();
                    break;
                case enModes.eResultShown:
                    NumbersAfterEqualMode(btn);
                    break;
                case enModes.eError:
                    MessageBox.Show("You are in the Error Mode", "Error");
                    break;
                default:
                    MessageBox.Show("You are in the Error Mode", "Error");
                    break;


            }


        }



        private void Operator_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (CurrentMode)
            {
                case enModes.eIdle:
                    ActivateErrorMode();
                    break;

                case enModes.eEnteringNumber:
                    OpDuringNumberMode(btn);

                    break;
                case enModes.eSelectFirstOperation:
                    UpdateCurrentOperation(btn);
                    break;
                case enModes.eChainOperator:
                    UpdateCurrentOperation(btn);
                    break;
                case enModes.eResultShown:
                    OpAfterEqualMode(btn);
                    break;
                case enModes.eError:
                    MessageBox.Show("You are in the Error Mode", "Error");
                    break;
                default:
                    MessageBox.Show("You are in the Error Mode", "Error");
                    break;

            }
        }

        private void btnEqualls_Click(object sender, EventArgs e)
        {
            switch (CurrentMode)
            {
                case enModes.eIdle:
                    ActivateErrorMode();
                    break;

                case enModes.eEnteringNumber:
                    EqualDuringNumbers();
                    break;

                case enModes.eSelectFirstOperation:
                    ActivateErrorMode();
                    break;
                case enModes.eChainOperator:
                    ActivateErrorMode();
                    break;
                case enModes.eResultShown:
                    EqualDuringNumbers();
                    break;
                case enModes.eError:
                    MessageBox.Show("You are in the Error Mode", "Error");
                    break;
                default:
                    MessageBox.Show("You are in the Error Mode", "Error");
                    break;

            }
        }

        void Reset()
        {
            if (CurrentMode == enModes.eError)
            {
                btn0.Enabled = true;
                btn1.Enabled = true;
                btn2.Enabled = true;
                btn3.Enabled = true;
                btn4.Enabled = true;
                btn5.Enabled = true;
                btn6.Enabled = true;
                btn7.Enabled = true;
                btn8.Enabled = true;
                btn9.Enabled = true;
                btnPlus.Enabled = true;
                btnMinus.Enabled = true;
                btnMultiply.Enabled = true;
                btnDivide.Enabled = true;
                btnBackSpace.Enabled = true;
                btnDot.Enabled = true;
                btnReverse.Enabled = true;
                btnEqualls.Enabled = true;
            }
            

            lbResult.Text = "0";
            UpdateCurrentValue();
            PreviousValue = 0;
            IsFirstOperation = true;
            CurrentOperation = enOperations.eAdd;
            CurrentMode = enModes.eIdle;


        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            if (!lbResult.Text.Contains("."))
            {
                lbResult.Text += ".";
            }
            UpdateCurrentValue();
        }

        private void btnReverse_Click(object sender, EventArgs e)
        {
            if (lbResult.Text != "0" && CurrentValue !=0 && CurrentMode !=  enModes.eError 
                && CurrentMode != enModes.eResultShown)
            {
                CurrentValue *= -1;
                lbResult.Text = CurrentValue.ToString();
            }
        }
    }
}
