﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using HealthManager.Annotations;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// トレーニングマスター画面VMクラス
    /// </summary>
    public class TrainingMasterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}