/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;

namespace ElmSharp
{
    /// <summary>
    /// An index widget gives you an index for fast access to whichever group of other UI items one might have.
    /// Inherits Layout
    /// </summary>
    public class Index : Layout
    {
        HashSet<IndexItem> _children = new HashSet<IndexItem>();
        SmartEvent _delayedChanged;

        /// <summary>
        /// Creates and initializes a new instance of Index class.
        /// </summary>
        /// <param name="parent">The parent is a given container which will be attached by Index as a child. It's <see cref="EvasObject"/> type.</param>
        public Index(EvasObject parent) : base(parent)
        {
            _delayedChanged = new SmartEvent(this, this.RealHandle, "delay,changed");
            _delayedChanged.On += _delayedChanged_On;
        }

        /// <summary>
        /// Changed will be triggered when the selected index item is changed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Sets or gets the auto hiding feature is enabled or not for a given index widget.
        /// </summary>
        public bool AutoHide
        {
            get
            {
                return !Interop.Elementary.elm_index_autohide_disabled_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_index_autohide_disabled_set(RealHandle, !value);
            }
        }

        /// <summary>
        /// Sets or gets a value whether horizontal mode is enabled or not.
        /// </summary>
        public bool IsHorizontal
        {
            get
            {
                return Interop.Elementary.elm_index_horizontal_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_index_horizontal_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the value of indicator's disabled status.
        /// </summary>
        public bool IndicatorVisible
        {
            get
            {
                return !Interop.Elementary.elm_index_indicator_disabled_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_index_indicator_disabled_set(RealHandle, !value);
            }
        }

        /// <summary>
        /// Sets or gets the omit feature is enabled or not for a given index widget.
        /// </summary>
        public bool OmitEnabled
        {
            get
            {
                return Interop.Elementary.elm_index_omit_enabled_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_index_omit_enabled_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Set a delay change time for index object.
        /// delay time is 0.2 sec by default.
        /// </summary>
        public double Delay
        {
            get
            {
                return Interop.Elementary.elm_index_delay_change_time_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_index_delay_change_time_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Gets or sets the items level for a given index widget.
        /// </summary>
        public int Level
        {
            get
            {
                return Interop.Elementary.elm_index_item_level_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_index_item_level_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Control standard_priority group of index.
        /// Priority group will be shown as many items as it can, and other group will be shown one character only.
        /// </summary>
        public int Priority
        {
            get
            {
                return Interop.Elementary.elm_index_standard_priority_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_index_standard_priority_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Gets the last selected item, for a given index widget.
        /// </summary>
        public IndexItem SelectedItem
        {
            get
            {
                IntPtr handle = Interop.Elementary.elm_index_selected_item_get(RealHandle, 0);
                return ItemObject.GetItemByHandle(handle) as IndexItem;
            }
        }

        /// <summary>
        /// Append a new item on a given index widget.
        /// </summary>
        /// <param name="label">the label which the item should be indexed</param>
        /// <returns>A object to the IndexItem added or null, on errors</returns>
        public IndexItem Append(string label)
        {
            IndexItem item = new IndexItem(label);
            item.Handle = Interop.Elementary.elm_index_item_append(RealHandle, label, null, (IntPtr)item.Id);
            return item;
        }

        /// <summary>
        /// Prepend a new item on a given index widget.
        /// </summary>
        /// <param name="label">the label which the item should be indexed</param>
        /// <returns>A handle to the item added or NULL, on errors</returns>
        public IndexItem Prepend(string label)
        {
            IndexItem item = new IndexItem(label);
            item.Handle = Interop.Elementary.elm_index_item_prepend(RealHandle, label, null, (IntPtr)item.Id);
            return item;
        }

        /// <summary>
        /// Insert a new item into the index object before item before.
        /// </summary>
        /// <param name="label">the label which the item should be indexed</param>
        /// <param name="before">The index item to insert after.</param>
        /// <returns>A object to the IndexItem added or null, on errors</returns>
        public IndexItem InsertBefore(string label, IndexItem before)
        {
            IndexItem item = new IndexItem(label);
            item.Handle = Interop.Elementary.elm_index_item_insert_before(RealHandle, before, label, null, (IntPtr)item.Id);
            return item;
        }

        /// <summary>
        /// Insert a new item into the index object after item after.
        /// </summary>
        /// <param name="label">the label which the item should be indexed</param>
        /// <param name="after">The index item to insert after.</param>
        /// <returns>A object to the IndexItem added or null, on errors</returns>
        public IndexItem InsertAfter(string label, IndexItem after)
        {
            IndexItem item = new IndexItem(label);
            item.Handle = Interop.Elementary.elm_index_item_insert_after(RealHandle, after, label, null, (IntPtr)item.Id);
            return item;
        }

        /// <summary>
        /// Flush the changes made to the index items so they work correctly.
        /// </summary>
        /// <param name="level">The index level (one of 0 or 1) where changes were made</param>
        public void Update(int level)
        {
            Interop.Elementary.elm_index_level_go(RealHandle, level);
        }

        /// <summary>
        /// Removes all items from a given index widget.
        /// </summary>
        public void Clear()
        {
            Interop.Elementary.elm_index_item_clear(RealHandle);
        }

        /// <summary>
        /// Creates a widget handle.
        /// </summary>
        /// <param name="parent">Parent EvasObject</param>
        /// <returns>Handle IntPtr</returns>
        protected override IntPtr CreateHandle(EvasObject parent)
        {
            IntPtr handle = Interop.Elementary.elm_layout_add(parent.Handle);
            Interop.Elementary.elm_layout_theme_set(handle, "layout", "elm_widget", "default");

            RealHandle = Interop.Elementary.elm_index_add(handle);
            Interop.Elementary.elm_object_part_content_set(handle, "elm.swallow.content", RealHandle);

            return handle;
        }

        void _delayedChanged_On(object sender, EventArgs e)
        {
            SelectedItem?.SendSelected();
            Changed?.Invoke(this, e);
        }

        void AddInternal(IndexItem item)
        {
            _children.Add(item);
            item.Deleted += Item_Deleted;
        }

        void Item_Deleted(object sender, EventArgs e)
        {
            _children.Remove((IndexItem)sender);
        }
    }
}