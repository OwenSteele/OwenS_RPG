﻿using Engine.Models;
using System;

namespace Engine.Actions
{
    public abstract class BaseAction
    {
        protected readonly GameItem _itemsInUse;

        public event EventHandler<string> OnActionPerformed;

        protected BaseAction(GameItem itemInUse)
        {
            _itemsInUse = itemInUse;
        }

        protected void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
