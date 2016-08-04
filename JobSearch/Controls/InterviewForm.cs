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
    class InterviewForm : RelativePanel
    {
        private ViewModels.MainPageViewModel ViewModel = ViewModels.MainPageViewModel.Instance;
        private TextBox ViaBox;
        private TextBox InterviewerBox;
        private TextBox NotesBox;
        private CalendarDatePicker DatePicker;
        private TimePicker TimePicker;
        private Button OkayButton;
        private Button CancelButton;

        public string MethodName { get; set; }

        public InterviewForm() : base()
        {
            // create controls
            ViaBox = new TextBox();
            InterviewerBox = new TextBox();
            NotesBox = new TextBox();
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
            ViaBox.MinWidth = 150;
            ViaBox.Margin = stdMargin; // new Thickness(0, 0, 8, 16);
            ViaBox.TextWrapping = TextWrapping.Wrap;
            ViaBox.Header = "Via";
            InterviewerBox.MinWidth = 150;
            InterviewerBox.Margin = stdMargin; // new Thickness(8, 0, 0, 16);
            InterviewerBox.TextWrapping = TextWrapping.Wrap;
            InterviewerBox.Header = "Interviewer";
            NotesBox.MinWidth = 250;
            NotesBox.MinHeight = 75;
            NotesBox.Margin = new Thickness(0, 8, 0, 0);
            NotesBox.TextWrapping = TextWrapping.Wrap;
            NotesBox.Header = "Notes";
            DatePicker.MinWidth = 150;
            DatePicker.Header = "Date";
            DatePicker.Margin = stdMargin; // new Thickness(0, 0, 8, 16);
            TimePicker.MinWidth = 150;
            TimePicker.Header = "Time";
            TimePicker.Margin = stdMargin; // new Thickness(8, 0, 0, 16);
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
            Interaction.GetBehaviors(ViaBox).Add(keyBehavior);
            Interaction.GetBehaviors(InterviewerBox).Add(keyBehavior);
            Interaction.GetBehaviors(NotesBox).Add(keyBehavior);

            // position controls in this RelativePanel
            ViaBox.SetValue(RelativePanel.AlignTopWithPanelProperty, true);
            ViaBox.SetValue(RelativePanel.AlignLeftWithPanelProperty, true);
            InterviewerBox.SetValue(RelativePanel.RightOfProperty, ViaBox);
            InterviewerBox.SetValue(RelativePanel.AlignTopWithProperty, ViaBox);
            InterviewerBox.SetValue(RelativePanel.AlignRightWithPanelProperty, true);
            DatePicker.SetValue(RelativePanel.BelowProperty, ViaBox);
            DatePicker.SetValue(RelativePanel.AlignLeftWithProperty, ViaBox);
            TimePicker.SetValue(RelativePanel.BelowProperty, InterviewerBox);
            TimePicker.SetValue(RelativePanel.RightOfProperty, DatePicker);
            TimePicker.SetValue(RelativePanel.AlignRightWithPanelProperty, true);
            NotesBox.SetValue(RelativePanel.BelowProperty, DatePicker);
            NotesBox.SetValue(RelativePanel.AlignLeftWithProperty, DatePicker);
            NotesBox.SetValue(RelativePanel.AlignRightWithProperty, TimePicker);
            OkayButton.SetValue(RelativePanel.BelowProperty, NotesBox);
            OkayButton.SetValue(RelativePanel.LeftOfProperty, CancelButton);
            CancelButton.SetValue(RelativePanel.BelowProperty, NotesBox);
            CancelButton.SetValue(RelativePanel.AlignRightWithProperty, NotesBox);

            // add controls to this RelativePanel
            this.Children.Add(ViaBox);
            this.Children.Add(InterviewerBox);
            this.Children.Add(DatePicker);
            this.Children.Add(TimePicker);
            this.Children.Add(NotesBox);
            this.Children.Add(OkayButton);
            this.Children.Add(CancelButton);
        }

        public void Focus()
        {
            ViaBox.Focus(FocusState.Programmatic);
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            // note that exceptions cannot be caught here due to the way the ViewModel method is invoked
            ViewModel.GetType().GetMethod(MethodName)
                .Invoke(ViewModel, new object[] { ViaBox.Text, InterviewerBox.Text, DatePicker.Date?.Date, TimePicker.Time, NotesBox.Text });
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
            ViaBox.Text = "";
            InterviewerBox.Text = "";
            DatePicker.Date = null;
            TimePicker.Time = default(TimeSpan);
            NotesBox.Text = "";
        }
    }
}
