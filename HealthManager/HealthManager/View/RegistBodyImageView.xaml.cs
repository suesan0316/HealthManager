﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistBodyImageView : ContentPage
    {
        public RegistBodyImageView()
        {            
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}