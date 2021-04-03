﻿using System;
using System.Collections.Generic;
using System.Linq;
using AbstractPizzeriaBusinessLogic.Interfaces;
using AbstractPizzeriaBusinessLogic.ViewModels;
using AbstractPizzeriaBusinessLogic.BindingModel;
using AbstractPizzeriaFileImplement.Models;

namespace AbstractPizzeriaFileImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly FileDataListSingleton source;

        public OrderStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<OrderViewModel> GetFullList()
        {
            return source.Orders.Select(CreateModel).ToList();
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Orders
                .Where(rec => rec.PizzaId.ToString().Contains(model.PizzaId.ToString()))
                .Select(CreateModel).ToList();
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var order = source.Orders
                .FirstOrDefault(rec => rec.PizzaId == model.PizzaId || rec.Id == model.Id);
            return order != null ? CreateModel(order) : null;
        }

        public void Insert(OrderBindingModel model)
        {
            int maxId = source.Orders.Count > 0 ? source.Orders.Max(rec => rec.Id) : 0;
            var element = new Order { Id = maxId + 1 };
            source.Orders.Add(CreateModel(model, element));
        }

        public void Update(OrderBindingModel model)
        {
            var element = source.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Order did not found");
            }
            CreateModel(model, element);
        }

        public void Delete(OrderBindingModel model)
        {
            Order element = source.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                source.Orders.Remove(element);
            }
            else
            {
                throw new Exception("Element did not found");
            }
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.PizzaId = model.PizzaId;
            order.Quantity = model.Quantity;
            order.Cost = model.Cost;
            order.Status = model.Status;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            return order;
        }

        private OrderViewModel CreateModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                Quantity = order.Quantity,
                Cost = order.Cost,
                Status = order.Status,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                PizzaId = order.PizzaId,
                PizzaName = source.Pizzas.FirstOrDefault(a => a.Id == order.PizzaId)?.PizzaName
            };
        }
    }
}
