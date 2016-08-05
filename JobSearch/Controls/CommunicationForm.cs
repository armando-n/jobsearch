using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Behaviors;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
using System.Reflection;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace JobSearch.Controls
{
    class CommunicationForm : RelativePanel
    {
        private ViewModels.MainPageViewModel ViewModel = ViewModels.MainPageViewModel.Instance;
        private TextBox ToBox;
        private TextBox FromBox;
        private TextBox SubjectBox;
        private TextBox ViaBox;
        private TextBox DescriptionBox;
        private CalendarDatePicker DatePicker;
        private TimePicker TimePicker;
        private Button OkayButton;
        private Button CancelButton;

        public string MethodName { get; set; }

        public CommunicationForm() : base()
        {
            // create controls
            ToBox = new TextBox();
            FromBox = new TextBox();
            SubjectBox = new TextBox();
            ViaBox = new TextBox();
            DescriptionBox = new TextBox();
            DatePicker = new CalendarDatePicker();
            TimePicker = new TimePicker();
            OkayButton = new Button();
            CancelButton = new Button();
            Thickness stdMargin = new Thickness(8);

            // set control properties
            this.Background = new SolidColorBrush(Color.FromArgb(255, 242, 242, 242));
            this.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
            this.BorderThickness = new Thickness(1);
            this.Padding = new Thickness(16);
            InitStandardTextBox(ToBox, "To");
            InitStandardTextBox(FromBox, "From");
            InitStandardTextBox(SubjectBox, "Subject");
            InitStandardTextBox(ViaBox, "Via");
            DescriptionBox.MinWidth = 250;
            DescriptionBox.MinHeight = 75;
            DescriptionBox.Margin = new Thickness(0, 8, 0, 0);
            DescriptionBox.TextWrapping = TextWrapping.Wrap;
            DescriptionBox.Header = "Description";
            DatePicker.MinWidth = 150;
            DatePicker.Header = "Date";
            DatePicker.Margin = stdMargin;
            TimePicker.MinWidth = 150;
            TimePicker.Header = "Time";
            TimePicker.Margin = stdMargin;
            OkayButton.Click += Okay_Clicked;
            OkayButton.Content = "Add";
            OkayButton.Margin = new Thickness(8, 8, 8, 0);
            CancelButton.Click += Cancel_Clicked;
            CancelButton.Content = "Cancel";
            CancelButton.Margin = new Thickness(8, 8, 8, 0);

            // enable submit on Enter key for TextBox controls
            KeyBehavior keyBehavior = new KeyBehavior();
            keyBehavior.Key = Windows.System.VirtualKey.Enter;
            CallMethodAction callMethodAction = new CallMethodAction();
            callMethodAction.MethodName = "Okay_Clicked";
            callMethodAction.TargetObject = this;
            keyBehavior.Actions.Add(callMethodAction);
            Interaction.GetBehaviors(ToBox).Add(keyBehavior);
            Interaction.GetBehaviors(FromBox).Add(keyBehavior);
            Interaction.GetBehaviors(SubjectBox).Add(keyBehavior);
            Interaction.GetBehaviors(ViaBox).Add(keyBehavior);
            Interaction.GetBehaviors(DescriptionBox).Add(keyBehavior);

            // position controls in this RelativePanel
            ToBox.SetValue(RelativePanel.AlignTopWithPanelProperty, true);
            ToBox.SetValue(RelativePanel.AlignLeftWithPanelProperty, true);
            FromBox.SetValue(RelativePanel.RightOfProperty, ToBox);
            FromBox.SetValue(RelativePanel.AlignTopWithProperty, ToBox);
            FromBox.SetValue(RelativePanel.AlignRightWithPanelProperty, true);
            SubjectBox.SetValue(RelativePanel.BelowProperty, ToBox);
            SubjectBox.SetValue(RelativePanel.AlignLeftWithProperty, ToBox);
            ViaBox.SetValue(RelativePanel.BelowProperty, FromBox);
            ViaBox.SetValue(RelativePanel.RightOfProperty, SubjectBox);
            ViaBox.SetValue(RelativePanel.AlignRightWithPanelProperty, true);
            DatePicker.SetValue(RelativePanel.BelowProperty, SubjectBox);
            DatePicker.SetValue(RelativePanel.AlignLeftWithPanelProperty, true);
            TimePicker.SetValue(RelativePanel.BelowProperty, ViaBox);
            TimePicker.SetValue(RelativePanel.RightOfProperty, DatePicker);
            TimePicker.SetValue(RelativePanel.AlignRightWithPanelProperty, true);
            DescriptionBox.SetValue(RelativePanel.BelowProperty, DatePicker);
            DescriptionBox.SetValue(RelativePanel.AlignLeftWithProperty, DatePicker);
            DescriptionBox.SetValue(RelativePanel.AlignRightWithProperty, TimePicker);
            OkayButton.SetValue(RelativePanel.BelowProperty, DescriptionBox);
            OkayButton.SetValue(RelativePanel.LeftOfProperty, CancelButton);
            CancelButton.SetValue(RelativePanel.BelowProperty, DescriptionBox);
            CancelButton.SetValue(RelativePanel.AlignRightWithProperty, DescriptionBox);

            // add controls to this RelativePanel
            this.Children.Add(ToBox);
            this.Children.Add(FromBox);
            this.Children.Add(SubjectBox);
            this.Children.Add(ViaBox);
            this.Children.Add(DatePicker);
            this.Children.Add(TimePicker);
            this.Children.Add(DescriptionBox);
            this.Children.Add(OkayButton);
            this.Children.Add(CancelButton);
        }

        public void Focus()
        {
            ToBox.Focus(FocusState.Programmatic);
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            // note that exceptions cannot be caught here due to the way the ViewModel method is invoked
            ViewModel.GetType().GetMethod(MethodName)
                .Invoke(ViewModel
                , new object[] { ToBox.Text, FromBox.Text, SubjectBox.Text, ViaBox.Text, DatePicker.Date?.Date, TimePicker.Time, DescriptionBox.Text });
            ClearForm();
            CloseOpenPopups();
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            ClearForm();
            CloseOpenPopups();
        }

        private void CloseOpenPopups()
        {
            foreach (Popup popup in VisualTreeHelper.GetOpenPopups(Window.Current))
                popup.IsOpen = false;
        }

        private void ClearForm()
        {
            ToBox.Text = "";
            FromBox.Text = "";
            SubjectBox.Text = "";
            ViaBox.Text = "";
            DatePicker.Date = null;
            TimePicker.Time = default(TimeSpan);
            DescriptionBox.Text = "";
        }

        private void InitStandardTextBox(TextBox tb, string header = null)
        {
            tb.MinWidth = 150;
            tb.Margin = new Thickness(8);
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Header = header;
        }
    }
}
