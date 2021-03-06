﻿namespace YouTube_Views_CSharp
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows.Threading;

    /// <summary>
    /// Collection that enables thread stuff
    /// </summary>
    /// <typeparam name="T">The type of the collection</typeparam>
    public class ExtendedObservableCollection<T> : ObservableCollection<T>
        {
            /// <summary>
            /// Occurs when [collection changed].
            /// </summary>
            public override event NotifyCollectionChangedEventHandler CollectionChanged;
 
            /// <summary>
            /// Raises the <see cref="E:CollectionChanged" /> event.
            /// </summary>
            /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
            protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            {
                NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
                if (collectionChanged != null)
                {
                    foreach (NotifyCollectionChangedEventHandler nh in collectionChanged.GetInvocationList())
                    {
                        DispatcherObject dispObj = nh.Target as DispatcherObject;
                        if (dispObj != null)
                        {
                            Dispatcher dispatcher = dispObj.Dispatcher;
                            if (dispatcher != null && !dispatcher.CheckAccess())
                            {
                                dispatcher.BeginInvoke(
                                    (Action)(() => nh.Invoke(
                                        this,
                                        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))),
                                    DispatcherPriority.DataBind);
                                continue;
                            }
                        }

                        nh.Invoke(this, e);
                    }
                }
             }
        }    
}
