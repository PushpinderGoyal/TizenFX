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

namespace ElmSharp
{
    /// <summary>
    /// The Spinner is a widget that increase or decrease numeric values using arrow buttons, or edit values directly.
    /// Inherits <see cref="Layout"/>.
    /// </summary>
    public class Spinner : Layout
    {
        double _minimum = 0.0;
        double _maximum = 100.0;

        SmartEvent _changed;
        SmartEvent _delayedChanged;

        /// <summary>
        /// Creates and initializes a new instance of the Spinner class.
        /// </summary>
        /// <param name="parent">The parent of new Spinner instance</param>
        public Spinner(EvasObject parent) : base(parent)
        {
            _changed = new SmartEvent(this, this.RealHandle, "changed");
            _changed.On += (s, e) => ValueChanged?.Invoke(this, EventArgs.Empty);

            _delayedChanged = new SmartEvent(this, this.RealHandle, "delay,changed");
            _delayedChanged.On += (s, e) => DelayedValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// ValueChanged will be triggered whenever the spinner value is changed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        ///  DelayedValueChanged will be triggered after a short time when the value is changed.
        /// </summary>
        public event EventHandler DelayedValueChanged;

        /// <summary>
        /// Sets or gets the label format of the spinner.
        /// </summary>
        public string LabelFormat
        {
            get
            {
                return Interop.Elementary.elm_spinner_label_format_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_spinner_label_format_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the minimum value for the spinner.
        /// </summary>
        public double Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
                Interop.Elementary.elm_spinner_min_max_set(RealHandle, _minimum, _maximum);
            }
        }

        /// <summary>
        /// Sets or gets the maximum value for the spinner.
        /// </summary>
        public double Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
                Interop.Elementary.elm_spinner_min_max_set(RealHandle, _minimum, _maximum);
            }
        }

        /// <summary>
        /// Sets or gets the step that used to increment or decrement the spinner value.
        /// </summary>
        public double Step
        {
            get
            {
                return Interop.Elementary.elm_spinner_step_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_spinner_step_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the value displayed by the spinner.
        /// </summary>
        public double Value
        {
            get
            {
                return Interop.Elementary.elm_spinner_value_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_spinner_value_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the interval on time updates for an user mouse button hold on spinner widgets' arrows.
        /// </summary>
        public double Interval
        {
            get
            {
                return Interop.Elementary.elm_spinner_interval_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_spinner_interval_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the base for rounding.
        /// </summary>
        public double RoundBase
        {
            get
            {
                return Interop.Elementary.elm_spinner_base_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_spinner_base_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the round value for rounding.
        /// </summary>
        public int RoundValue
        {
            get
            {
                return Interop.Elementary.elm_spinner_round_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_spinner_round_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the wrap of a given spinner widget.
        /// </summary>
        /// <remarks>
        /// If wrap is disabled, when the user tries to increment the value, but displayed value plus step value is bigger than maximum value, the new value will be the maximum value.
        /// If wrap is enabled, when the user tries to increment the value, but displayed value plus step value is bigger than maximum value, the new value will be the minimum value.
        /// By default it's disabled.
        /// </remarks>
        public bool IsWrapEnabled
        {
            get
            {
                return Interop.Elementary.elm_spinner_wrap_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_spinner_wrap_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets whether the spinner can be directly edited by the user or not.
        /// </summary>
        /// <remarks>By default it is enabled</remarks>
        public bool IsEditable
        {
            get
            {
                return Interop.Elementary.elm_spinner_editable_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_spinner_editable_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Set a special string to display in the place of the numerical value.
        /// </summary>
        /// <param name="value">The numerical value to be replaced</param>
        /// <param name="label">The label to be used</param>
        public void AddSpecialValue(double value, string label)
        {
            Interop.Elementary.elm_spinner_special_value_add(RealHandle, value, label);
        }

        /// <summary>
        /// Remove a previously added special value, After this, the spinner will display the value itself instead of a label.
        /// </summary>
        /// <param name="value">The replaced numerical value</param>
        public void RemoveSpecialValue(double value)
        {
            Interop.Elementary.elm_spinner_special_value_del(RealHandle, value);
        }

        /// <summary>
        /// Get the special string display in the place of the numerical value.
        /// </summary>
        /// <param name="value">The replaced numerical value.</param>
        /// <returns>The value of the spinner which replaced numerical value with special string</returns>
        public string GetSpecialValue(double value)
        {
            return Interop.Elementary.elm_spinner_special_value_get(RealHandle, value);
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

            RealHandle = Interop.Elementary.elm_spinner_add(handle);
            Interop.Elementary.elm_object_part_content_set(handle, "elm.swallow.content", RealHandle);

            return handle;
        }
    }
}
