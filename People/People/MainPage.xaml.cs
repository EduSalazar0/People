﻿using People.Models;
using People.ViewModels;
using System.Collections.Generic;

namespace People;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
        BindingContext = new PersonViewModel();  
    }

   /* 
    * Codigo "comentado referente a la funcionalidad pasada de los botones"
    * public async void OnNewButtonClicked(object sender, EventArgs args)
    {
        statusMessage.Text = "";

        await App.PersonRepo.AddNewPerson(newPerson.Text);
        statusMessage.Text = App.PersonRepo.StatusMessage;
    }

    public async void OnGetButtonClicked(object sender, EventArgs args)
    {
        statusMessage.Text = "";

        List<Person> people = await App.PersonRepo.GetAllPeople();
        peopleList.ItemsSource = people;
    }
   */
}

