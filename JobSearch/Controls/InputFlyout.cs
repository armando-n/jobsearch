using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Behaviors;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
using System.Reflection;

namespace JobSearch.Controls
{
    class InputFlyout : Flyout
    {
        private ViewModels.MainPageViewModel ViewModel;
        private TextBox textBox;
        private Button okayButton;
        private Button cancelButton;

        public string MethodName { get; set; }
        public string OkayButtonText { get; set; }
        public string CancelButtonText { get; set; }

        public InputFlyout() : base()
        {
            ViewModel = ViewModels.MainPageViewModel.Instance;
            RelativePanel relativePanel = new RelativePanel();
            textBox = new TextBox();
            okayButton = new Button();
            cancelButton = new Button();

            OkayButtonText = "Add";
            CancelButtonText = "Cancel";
            textBox.MinWidth = 250;
            textBox.MinHeight = 75;
            textBox.TextWrapping = TextWrapping.Wrap;

            KeyBehavior keyBehavior = new KeyBehavior();
            keyBehavior.Key = Windows.System.VirtualKey.Enter;
            CallMethodAction callMethodAction = new CallMethodAction();
            callMethodAction.MethodName = "Okay_Clicked";
            callMethodAction.TargetObject = this;
            keyBehavior.Actions.Add(callMethodAction);
            Interaction.GetBehaviors(textBox).Add(keyBehavior);

            Thickness buttonMargin = new Thickness(8, 8, 8, 0);
            okayButton.Click += Okay_Clicked;
            okayButton.Content = OkayButtonText;
            okayButton.Margin = buttonMargin;
            cancelButton.Click += Cancel_Clicked;
            cancelButton.Content = CancelButtonText;
            cancelButton.Margin = buttonMargin;

            textBox.SetValue(RelativePanel.AlignTopWithPanelProperty, true);
            textBox.SetValue(RelativePanel.AlignLeftWithPanelProperty, true);
            okayButton.SetValue(RelativePanel.BelowProperty, textBox);
            okayButton.SetValue(RelativePanel.LeftOfProperty, cancelButton);
            cancelButton.SetValue(RelativePanel.BelowProperty, textBox);
            cancelButton.SetValue(RelativePanel.AlignRightWithProperty, textBox);

            relativePanel.Children.Add(textBox);
            relativePanel.Children.Add(okayButton);
            relativePanel.Children.Add(cancelButton);
            
            this.Content = relativePanel;
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.GetType().GetMethod(MethodName).Invoke(ViewModel, new[] { textBox.Text });
                textBox.Text = "";
                this.Hide();
            }
            catch (ArgumentNullException ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            textBox.Text = "";
            this.Hide();
        }

    }
}
