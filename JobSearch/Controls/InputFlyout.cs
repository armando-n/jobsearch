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
            //EventTriggerBehavior eventTriggerBehavior = new EventTriggerBehavior();
            //eventTriggerBehavior.EventName = "Click";
            //eventTriggerBehavior.Actions.Add(new CloseFlyoutAction());
            //Interaction.GetBehaviors(okayButton).Add(eventTriggerBehavior);

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

        //private string FindSiblingTextBox(DependencyObject obj)
        //{
        //    DependencyObject objParent = VisualTreeHelper.GetParent(obj);

        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(objParent); i++)
        //    {
        //        DependencyObject child = VisualTreeHelper.GetChild(objParent, i);
        //        if (child != null && child is TextBox)
        //        {
        //            TextBox tbox = child as TextBox;
        //            string tboxText = tbox.Text;
        //            tbox.Text = "";
        //            return tboxText;
        //        }
        //    }

        //    return null;
        //}

        //private string FindDescendantTextBox(DependencyObject obj, int level = 0)
        //{
        //    if (obj == null)
        //        return null;

        //    for (int i = 0; i < level; i++)
        //        System.Diagnostics.Debug.Write("    ");
        //    System.Diagnostics.Debug.Write(obj.GetType().ToString() + ": " + (obj as FrameworkElement).Name + "\n");

        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        //    {
        //        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
        //        //if (child != null && child is Button && (child as Button).Name == "RequirementFlyoutButton")
        //        //{
        //        //    System.Diagnostics.Debug.Write("REQUIREMENT FLYOUT BUTTON FOUND!!!!!");
        //        //    Flyout flyout = (child as Button).Flyout as Flyout;
        //        //    string result = FindDescendantTextBox(flyout.Content);
        //        //    if (result != null)
        //        //        return result;
        //        //}
        //        if (child != null && child is TextBox/* && (child as TextBox).Name == "RequirementBox"*/)
        //        {
        //            TextBox tbox = child as TextBox;
        //            string tboxText = tbox.Text;
        //            tbox.Text = "";
        //            return tboxText;
        //        }
        //        else
        //        {
        //            string result = FindDescendantTextBox(child, level + 1);
        //            if (result != null)
        //                return result;
        //        }
        //    }

        //    return null;
        //}

        // The XAML below is what the contructor above creates; however, the CloseFlyoutAction is no longer necessary (I can hide flyout directly)
        //<Button.Flyout>
        //    <Flyout x:Name="RequirementFlyout">
        //        <StackPanel>
        //            <TextBox x:Name="RequirementBox" MinWidth="250" MinHeight="75" TextWrapping="Wrap">
        //                <Interactivity:Interaction.Behaviors>
        //                    <Behaviors:KeyBehavior Key = "Enter" >
        //                        < Core:CallMethodAction MethodName = "AddRequirement_Entered" TargetObject="{Binding ElementName=JobsPage}" />
        //                    </Behaviors:KeyBehavior>
        //                </Interactivity:Interaction.Behaviors>
        //            </TextBox>
        //            <Button x:Name="AddRequirementButton" Click="AddRequirement_Clicked">
        //                <Interactivity:Interaction.Behaviors>
        //                    <Core:EventTriggerBehavior EventName = "Click" >
        //                        < Behaviors:CloseFlyoutAction />
        //                    </Core:EventTriggerBehavior>
        //                </Interactivity:Interaction.Behaviors>
        //                Add
        //            </Button>
        //        </StackPanel>
        //    </Flyout>
        //</Button.Flyout>
    }
}
