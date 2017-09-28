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
    /// The NaviframeEventArgs is a event args class for navi frame.
    /// Inherits EventArgs
    /// </summary>
    public class NaviframeEventArgs : EventArgs
    {
        /// <summary>
        /// Sets or gets the content object. The name of content part is "elm.swallow.content".
        /// </summary>
        public EvasObject Content { get; set; }
    }
    /// <summary>
    /// Naviframe is a widget to stands for navigation frame. It's a views manager for applications.
    /// Inherits Widget
    /// </summary>
    public class Naviframe : Widget
    {
        SmartEvent _transitionFinished;
        readonly List<NaviItem> _itemStack = new List<NaviItem>();

        /// <summary>
        /// Creates and initializes a new instance of Naviframe class.
        /// </summary>
        /// <param name="parent">The parent is a given container which will be attached by Naviframe as a child. It's <see cref="EvasObject"/> type.</param>
        public Naviframe(EvasObject parent) : base(parent)
        {
            _transitionFinished = new SmartEvent(this, this.RealHandle, "transition,finished");
            _transitionFinished.On += (s, e) => AnimationFinished?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Popped will be triggered when NaviItem is removed.
        /// </summary>
        /// <remarks>
        /// It is always called when NaviItem was removed.
        /// (even if removed by NaviItem.Delete())
        /// This event will be invoked in progress of Pop/Delete operation.
        /// After called Popped event, Pop/Delete method will be returned
        /// </remarks>
        public event EventHandler<NaviframeEventArgs> Popped;

        /// <summary>
        /// AnimationFinished will be triggered when animation is finished.
        /// </summary>
        public event EventHandler AnimationFinished;

        /// <summary>
        /// Gets the list of navi item
        /// </summary>
        public IReadOnlyList<NaviItem> NavigationStack
        {
            get { return _itemStack; }
        }

        /// <summary>
        /// Sets or gets the the preserve content objects when items are popped.
        /// </summary>
        public bool PreserveContentOnPop
        {
            get
            {
                return Interop.Elementary.elm_naviframe_content_preserve_on_pop_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_naviframe_content_preserve_on_pop_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets whether the default back button is enabled
        /// </summary>
        public bool DefaultBackButtonEnabled
        {
            get
            {
                return Interop.Elementary.elm_naviframe_prev_btn_auto_pushed_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_naviframe_prev_btn_auto_pushed_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Push a new item to the top of the naviframe stack and show it.
        /// The title and style are null.
        /// </summary>
        /// <param name="content">The main content object. The name of content part is "elm.swallow.content".</param>
        /// <returns>The created item or null upon failure.</returns>
        public NaviItem Push(EvasObject content)
        {
            return Push(content, null);
        }

        /// <summary>
        /// Push a new item to the top of the naviframe stack and show it.
        /// The style are null.
        /// </summary>
        /// <param name="content">The main content object. The name of content part is "elm.swallow.content".</param>
        /// <param name="title">The current item title. null would be default.</param>
        /// <returns></returns>
        public NaviItem Push(EvasObject content, string title)
        {
            return Push(content, title, null);
        }

        /// <summary>
        /// Push a new item to the top of the naviframe stack and show it.
        /// </summary>
        /// <param name="content">The main content object. The name of content part is "elm.swallow.content".</param>
        /// <param name="title">The current item title. null would be default.</param>
        /// <param name="style">The current item style name. null would be default.</param>
        /// <returns>The created item or null upon failure.</returns>
        public NaviItem Push(EvasObject content, string title, string style)
        {
            IntPtr item = Interop.Elementary.elm_naviframe_item_push(RealHandle, title, IntPtr.Zero, IntPtr.Zero, content.Handle, style);
            NaviItem naviItem = NaviItem.FromNativeHandle(item, content);
            _itemStack.Add(naviItem);
            naviItem.Popped += ItemPoppedHandler;
            return naviItem;
        }

        /// <summary>
        /// Insert a new item into the naviframe before item.
        /// The title is "" and the style is null.
        /// </summary>
        /// <param name="before">The item which the new item is inserted before.</param>
        /// <param name="content">The main content object. The name of content part is "elm.swallow.content".</param>
        /// <returns>The created item or null upon failure.</returns>
        public NaviItem InsertBefore(NaviItem before, EvasObject content)
        {
            return InsertBefore(before, content, "");
        }

        /// <summary>
        /// Insert a new item into the naviframe before item.
        /// The style is null.
        /// </summary>
        /// <param name="before">The item which the new item is inserted before.</param>
        /// <param name="content">The main content object. The name of content part is "elm.swallow.content".</param>
        /// <param name="title">The current item title. null would be default.</param>
        /// <returns>The created item or null upon failure.</returns>
        public NaviItem InsertBefore(NaviItem before, EvasObject content, string title)
        {
            return InsertBefore(before, content, title, null);
        }

        /// <summary>
        /// Insert a new item into the naviframe before item.
        /// </summary>
        /// <param name="before">The item which the new item is inserted before.</param>
        /// <param name="content">The main content object. The name of content part is "elm.swallow.content".</param>
        /// <param name="title">The current item title. null would be default.</param>
        /// <param name="style">The current item style name. null would be default.</param>
        /// <returns>The created item or null upon failure.</returns>
        public NaviItem InsertBefore(NaviItem before, EvasObject content, string title, string style)
        {
            IntPtr item = Interop.Elementary.elm_naviframe_item_insert_before(RealHandle, before, title, IntPtr.Zero, IntPtr.Zero, content, null);
            NaviItem naviItem = NaviItem.FromNativeHandle(item, content);
            int idx = _itemStack.IndexOf(before);
            _itemStack.Insert(idx, naviItem);
            naviItem.Popped += ItemPoppedHandler;
            return naviItem;
        }

        /// <summary>
        /// Insert a new item into the naviframe after item.
        /// The title is "" and the style is null.
        /// </summary>
        /// <param name="after">The item which the new item is inserted after.</param>
        /// <param name="content">The main content object. The name of content part is "elm.swallow.content".</param>
        /// <returns>The created item or null upon failure.</returns>
        public NaviItem InsertAfter(NaviItem after, EvasObject content)
        {
            return InsertAfter(after, content, "");
        }

        /// <summary>
        /// Insert a new item into the naviframe after item.
        /// The style is null.
        /// </summary>
        /// <param name="after">The item which the new item is inserted after.</param>
        /// <param name="content">The main content object. The name of content part is "elm.swallow.content".</param>
        /// <param name="title">The current item title. null would be default.</param>
        /// <returns>The created item or null upon failure.</returns>
        public NaviItem InsertAfter(NaviItem after, EvasObject content, string title)
        {
            return InsertAfter(after, content, title, null);
        }

        /// <summary>
        /// Insert a new item into the naviframe after item.
        /// </summary>
        /// <param name="after">The item which the new item is inserted after.</param>
        /// <param name="content">The main content object. The name of content part is "elm.swallow.content".</param>
        /// <param name="title">The current item title. null would be default.</param>
        /// <param name="style">The current item style name. null would be default.</param>
        /// <returns>The created item or null upon failure.</returns>
        public NaviItem InsertAfter(NaviItem after, EvasObject content, string title, string style)
        {
            IntPtr item = Interop.Elementary.elm_naviframe_item_insert_after(RealHandle, after, title, IntPtr.Zero, IntPtr.Zero, content, null);
            NaviItem naviItem = NaviItem.FromNativeHandle(item, content);
            int idx = _itemStack.IndexOf(after);
            _itemStack.Insert(idx + 1, naviItem);
            naviItem.Popped += ItemPoppedHandler;
            return naviItem;
        }

        /// <summary>
        /// Pop an item that is on top of the stack.
        /// </summary>
        public void Pop()
        {
            Interop.Elementary.elm_naviframe_item_pop(RealHandle);
        }

        /// <summary>
        /// Creates a widget handle.
        /// </summary>
        /// <param name="parent">Parent EvasObject</param>
        /// <returns>Handle IntPtr</returns>
        protected override IntPtr CreateHandle(EvasObject parent)
        {
            IntPtr handle = Interop.Elementary.elm_layout_add(parent);
            Interop.Elementary.elm_layout_theme_set(handle, "layout", "elm_widget", "default");

            RealHandle = Interop.Elementary.elm_naviframe_add(handle);
            Interop.Elementary.elm_object_part_content_set(handle, "elm.swallow.content", RealHandle);

            return handle;
        }

        void ItemPoppedHandler(object sender, EventArgs e)
        {
            NaviItem item = sender as NaviItem;
            if (item == null)
                return;
            _itemStack.Remove(item);
            Popped?.Invoke(this, new NaviframeEventArgs() { Content = item.Content });
        }
    }
}
