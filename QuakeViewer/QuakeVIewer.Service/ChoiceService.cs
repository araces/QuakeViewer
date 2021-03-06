﻿//
//  Souce	Path	ChoiceService.cs
//  create	Date	2017-2-12 15:54:35		
//  created	By	Ares.Zhao
//
using System;
using System.Collections.Generic;
using System.Linq;
using QuakeViewer.DAL;
using QuakeViewer.Models;
using QuakeViewer.Utils;

namespace QuakeViewer.Service
{
    public class ChoiceService
    {
        ChroiceContext chroiceContext { get; set; }


        public ChoiceService() : this(new ChroiceContext())
        {
        }

        public ChoiceService(ChroiceContext _chroiceContext)
        {
            chroiceContext = _chroiceContext;
        }

        public Choice GetChoiceByUserId(string userId, int fromType)
        {
            var choice = chroiceContext.Choices.FirstOrDefault(p => p.UserId == userId && p.FromType == fromType);
            return choice;
        }

        public List<Choice> GetChoiceByTime(DateTime startTime, DateTime endTime)
        {
            var choices = chroiceContext.Choices.Where(p => p.CreateDate > startTime && p.CreateDate < endTime);



            return choices.OrderByDescending(p => p.CreateDate).ToList();
        }

        public Choice GetChoiceById(string id)
        {
            return chroiceContext.Choices.FirstOrDefault(p => p.Id == id);
        }

        public void SaveChoice(Choice choice)
        {
            if (string.IsNullOrEmpty(choice.Id))
            {
                choice.Id = StringHelper.GuidString();
            }


            chroiceContext.Choices.Add(choice);
            chroiceContext.SaveChanges();
        }

    }
}
