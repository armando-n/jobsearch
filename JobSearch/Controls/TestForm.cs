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
    class TestForm : RelativePanel
    {
        private ViewModels.MainPageViewModel ViewModel = ViewModels.MainPageViewModel.Instance;
        private TextBox TypeBox;
        private TextBox NotesBox;
        private CalendarDatePicker DatePicker;
        private TimePicker TimePicker;
        private Button OkayButton;
        private Button CancelButton;

        public string MethodName { get; set; }
        public string OkayButtonText { get; set; }
        public string CancelButtonText { get; set; }

        public TestForm() : base()
        {
            // create controls
            TypeBox = new TextBox();
            NotesBox = new TextBox();
            DatePicker = new CalendarDatePicker();
            TimePicker = new TimePicker();
            OkayButton = new Button();
            CancelButton = new Button();

            // set control properties
            this.Background = new SolidColorBrush(Color.FromArgb(255, 242, 242, 242));
            this.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
            this.BorderThickness = new Thickness(1);
            this.Padding = new Thickness(16);
            OkayButtonText = "Add";
            OkayButton.Click += Okay_Clicked;
            OkayButton.Content = OkayButtonText;
            OkayButton.Margin = new Thickness(8, 8, 8, 0);
            CancelButtonText = "Cancel";
            CancelButton.Click += Cancel_Clicked;
            CancelButton.Content = CancelButtonText;
            CancelButton.Margin = new Thickness(8, 8, 8, 0);
            TypeBox.MinWidth = 150;
            TypeBox.Margin = new Thickness(0, 0, 8, 16);
            TypeBox.TextWrapping = TextWrapping.Wrap;
            TypeBox.Header = "Type";
            NotesBox.MinWidth = 250;
            NotesBox.MinHeight = 75;
            NotesBox.TextWrapping = TextWrapping.Wrap;
            NotesBox.Header = "Notes";
            DatePicker.MinWidth = 150;
            DatePicker.Header = "Date";
            DatePicker.Margin = new Thickness(8, 0, 8, 16);
            TimePicker.MinWidth = 150;
            TimePicker.Header = "Time";
            TimePicker.Margin = new Thickness(8, 0, 0, 16);

            // enable submit on Enter key for TextBox controls
            KeyBehavior keyBehavior = new KeyBehavior();
            keyBehavior.Key = Windows.System.VirtualKey.Enter;
            CallMethodAction callMethodAction = new CallMethodAction();
            callMethodAction.MethodName = "Okay_Clicked";
            callMethodAction.TargetObject = this;
            keyBehavior.Actions.Add(callMethodAction);
            Interaction.GetBehaviors(TypeBox).Add(keyBehavior);
            Interaction.GetBehaviors(NotesBox).Add(keyBehavior);

            // position controls in this RelativePanel
            TypeBox.SetValue(RelativePanel.AlignTopWithPanelProperty, true);
            TypeBox.SetValue(RelativePanel.AlignLeftWithPanelProperty, true);
            DatePicker.SetValue(RelativePanel.RightOfProperty, TypeBox);
            DatePicker.SetValue(RelativePanel.AlignTopWithProperty, TypeBox);
            TimePicker.SetValue(RelativePanel.RightOfProperty, DatePicker);
            TimePicker.SetValue(RelativePanel.AlignTopWithProperty, TypeBox);
            TimePicker.SetValue(RelativePanel.AlignRightWithPanelProperty, true);
            NotesBox.SetValue(RelativePanel.BelowProperty, TypeBox);
            NotesBox.SetValue(RelativePanel.AlignLeftWithProperty, TypeBox);
            NotesBox.SetValue(RelativePanel.AlignRightWithProperty, TimePicker);
            OkayButton.SetValue(RelativePanel.BelowProperty, NotesBox);
            OkayButton.SetValue(RelativePanel.LeftOfProperty, CancelButton);
            CancelButton.SetValue(RelativePanel.BelowProperty, NotesBox);
            CancelButton.SetValue(RelativePanel.AlignRightWithProperty, NotesBox);

            // add controls to this RelativePanel
            this.Children.Add(TypeBox);
            this.Children.Add(DatePicker);
            this.Children.Add(TimePicker);
            this.Children.Add(NotesBox);
            this.Children.Add(OkayButton);
            this.Children.Add(CancelButton);
        }

        public void Focus()
        {
            TypeBox.Focus(FocusState.Programmatic);
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            // note that exceptions cannot be caught here due to the way the ViewModel method is invoked
            ViewModel.GetType().GetMethod(MethodName)
                .Invoke(ViewModel, new object[] { TypeBox.Text, DatePicker.Date?.Date, TimePicker.Time, NotesBox.Text });
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
            TypeBox.Text = "";
            DatePicker.Date = null;
            TimePicker.Time = default(TimeSpan);
            NotesBox.Text = "";
        }
    }
}
