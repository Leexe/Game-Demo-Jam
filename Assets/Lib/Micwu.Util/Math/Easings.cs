/*
 * Created by C.J. Kimberlin
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2019
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * 
 * TERMS OF USE - EASING EQUATIONS
 * Open source under the BSD License.
 * Copyright (c)2001 Robert Penner
 * All rights reserved.
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE 
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 *
 * ============= Description =============
 *
 * Below is an example of how to use the easing functions in the file. There is a getting function that will return the function
 * from an enum. This is useful since the enum can be exposed in the editor and then the function queried during Start().
 * 
 * EasingFunction.Ease ease = EasingFunction.Ease.InOutQuad;
 * EasingFunction.EasingFunc func = GetEasingFunction(ease);
 * 
 * float t = func(0, 10, 0.67f);
 * 
 * EasingFunction.EaseingFunc derivativeFunc = GetEasingFunctionDerivative(ease);
 * 
 * float derivativet = derivativeFunc(0, 10, 0.67f);
 */

// micwu: refactored a bit but functionality should be untouched

using System;
using UnityEngine;

namespace Micwu.Util.Math
{
    public static class EaseFunc
    {
        [Serializable]
        public enum Ease
        {
            InQuad = 0,
            OutQuad,
            InOutQuad,
            InCubic,
            OutCubic,
            InOutCubic,
            InQuart,
            OutQuart,
            InOutQuart,
            InQuint,
            OutQuint,
            InOutQuint,
            InSine,
            OutSine,
            InOutSine,
            InExpo,
            OutExpo,
            InOutExpo,
            InCirc,
            OutCirc,
            InOutCirc,
            Linear,
            Spring,
            InBounce,
            OutBounce,
            InOutBounce,
            InBack,
            OutBack,
            InOutBack,
            InElastic,
            OutElastic,
            InOutElastic,
        }

        private const float NATURAL_LOG_OF_2 = 0.693147181f;

        //
        // Easing functions
        //

        public static float Linear(float t, float start = 0f, float end = 1f)
        {
            return Mathf.Lerp(start, end, t);
        }

        public static float Spring(float t, float start = 0f, float end = 1f)
        {
            t = Mathf.Clamp01(t);
            t = (Mathf.Sin(t * Mathf.PI * (0.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t) * (1f + (1.2f * (1f - t)));
            return start + (end - start) * t;
        }

        public static float InQuad(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return end * t * t + start;
        }

        public static float OutQuad(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return -end * t * (t - 2) + start;
        }

        public static float InOutQuad(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;
            if (t < 1) return end * 0.5f * t * t + start;
            t--;
            return -end * 0.5f * (t * (t - 2) - 1) + start;
        }

        public static float InCubic(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return end * t * t * t + start;
        }

        public static float OutCubic(float t, float start = 0f, float end = 1f)
        {
            t--;
            end -= start;
            return end * (t * t * t + 1) + start;
        }

        public static float InOutCubic(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;
            if (t < 1) return end * 0.5f * t * t * t + start;
            t -= 2;
            return end * 0.5f * (t * t * t + 2) + start;
        }

        public static float InQuart(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return end * t * t * t * t + start;
        }

        public static float OutQuart(float t, float start = 0f, float end = 1f)
        {
            t--;
            end -= start;
            return -end * (t * t * t * t - 1) + start;
        }

        public static float InOutQuart(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;
            if (t < 1) return end * 0.5f * t * t * t * t + start;
            t -= 2;
            return -end * 0.5f * (t * t * t * t - 2) + start;
        }

        public static float InQuint(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return end * t * t * t * t * t + start;
        }

        public static float OutQuint(float t, float start = 0f, float end = 1f)
        {
            t--;
            end -= start;
            return end * (t * t * t * t * t + 1) + start;
        }

        public static float InOutQuint(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;
            if (t < 1) return end * 0.5f * t * t * t * t * t + start;
            t -= 2;
            return end * 0.5f * (t * t * t * t * t + 2) + start;
        }

        public static float InSine(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return -end * Mathf.Cos(t * (Mathf.PI * 0.5f)) + end + start;
        }

        public static float OutSine(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return end * Mathf.Sin(t * (Mathf.PI * 0.5f)) + start;
        }

        public static float InOutSine(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return -end * 0.5f * (Mathf.Cos(Mathf.PI * t) - 1) + start;
        }

        public static float InExpo(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return end * Mathf.Pow(2, 10 * (t - 1)) + start;
        }

        public static float OutExpo(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * t) + 1) + start;
        }

        public static float InOutExpo(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;
            if (t < 1) return end * 0.5f * Mathf.Pow(2, 10 * (t - 1)) + start;
            t--;
            return end * 0.5f * (-Mathf.Pow(2, -10 * t) + 2) + start;
        }

        public static float InCirc(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return -end * (Mathf.Sqrt(1 - t * t) - 1) + start;
        }

        public static float OutCirc(float t, float start = 0f, float end = 1f)
        {
            t--;
            end -= start;
            return end * Mathf.Sqrt(1 - t * t) + start;
        }

        public static float InOutCirc(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;
            if (t < 1) return -end * 0.5f * (Mathf.Sqrt(1 - t * t) - 1) + start;
            t -= 2;
            return end * 0.5f * (Mathf.Sqrt(1 - t * t) + 1) + start;
        }

        public static float InBounce(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            float d = 1f;
            return end - OutBounce(0, end, d - t) + start;
        }

        public static float OutBounce(float t, float start = 0f, float end = 1f)
        {
            t /= 1f;
            end -= start;
            if (t < (1 / 2.75f))
            {
                return end * (7.5625f * t * t) + start;
            }
            else if (t < (2 / 2.75f))
            {
                t -= 1.5f / 2.75f;
                return end * (7.5625f * t * t + .75f) + start;
            }
            else if (t < (2.5 / 2.75f))
            {
                t -= 2.25f / 2.75f;
                return end * (7.5625f * t * t + .9375f) + start;
            }
            else
            {
                t -= 2.625f / 2.75f;
                return end * (7.5625f * t * t + .984375f) + start;
            }
        }

        public static float InOutBounce(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            float d = 1f;
            if (t < d * 0.5f) return InBounce(0, end, t * 2) * 0.5f + start;
            else return OutBounce(0, end, t * 2 - d) * 0.5f + end * 0.5f + start;
        }

        public static float InBack(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            float s = 1.70158f;
            return end * t * t * ((s + 1) * t - s) + start;
        }

        public static float OutBack(float t, float start = 0f, float end = 1f)
        {
            float s = 1.70158f;
            end -= start;
            t--;
            return end * (t * t * ((s + 1) * t + s) + 1) + start;
        }

        public static float InOutBack(float t, float start = 0f, float end = 1f)
        {
            float s = 1.70158f;
            end -= start;
            t *= 2f;
            if (t < 1)
            {
                s *= 1.525f;
                return end * 0.5f * (t * t * ((s + 1) * t - s)) + start;
            }
            t -= 2;
            s *= 1.525f;
            return end * 0.5f * (t * t * ((s + 1) * t + s) + 2) + start;
        }

        public static float InElastic(float t, float start = 0f, float end = 1f)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (t == 0) return start;

            if ((t /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return -(a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + start;
        }

        public static float OutElastic(float t, float start = 0f, float end = 1f)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (t == 0) return start;

            if ((t /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + end + start;
        }

        public static float InOutElastic(float t, float start = 0f, float end = 1f)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (t == 0) return start;

            if ((t /= d * 0.5f) == 2) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (t < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + start;
            return a * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }

        //
        // These are derived functions that the motor can use to get the speed at a specific time.
        //
        // The easing functions all work with a normalized time (0 to 1) and the returned t here
        // reflects that. ts returned here should be divided by the actual time.
        //
        // TODO: These functions have not had the testing they deserve. If there is odd behavior around
        //       dash speeds then this would be the first place I'd look.

        public static float LinearD(float t, float start = 0f, float end = 1f)
        {
            return end - start;
        }

        public static float InQuadD(float t, float start = 0f, float end = 1f)
        {
            return 2f * (end - start) * t;
        }

        public static float OutQuadD(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return -end * t - end * (t - 2);
        }

        public static float InOutQuadD(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;

            if (t < 1)
            {
                return end * t;
            }

            t--;

            return end * (1 - t);
        }

        public static float InCubicD(float t, float start = 0f, float end = 1f)
        {
            return 3f * (end - start) * t * t;
        }

        public static float OutCubicD(float t, float start = 0f, float end = 1f)
        {
            t--;
            end -= start;
            return 3f * end * t * t;
        }

        public static float InOutCubicD(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;

            if (t < 1)
            {
                return 3f / 2f * end * t * t;
            }

            t -= 2;

            return 3f / 2f * end * t * t;
        }

        public static float InQuartD(float t, float start = 0f, float end = 1f)
        {
            return 4f * (end - start) * t * t * t;
        }

        public static float OutQuartD(float t, float start = 0f, float end = 1f)
        {
            t--;
            end -= start;
            return -4f * end * t * t * t;
        }

        public static float InOutQuartD(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;

            if (t < 1)
            {
                return 2f * end * t * t * t;
            }

            t -= 2;

            return -2f * end * t * t * t;
        }

        public static float InQuintD(float t, float start = 0f, float end = 1f)
        {
            return 5f * (end - start) * t * t * t * t;
        }

        public static float OutQuintD(float t, float start = 0f, float end = 1f)
        {
            t--;
            end -= start;
            return 5f * end * t * t * t * t;
        }

        public static float InOutQuintD(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;

            if (t < 1)
            {
                return 5f / 2f * end * t * t * t * t;
            }

            t -= 2;

            return 5f / 2f * end * t * t * t * t;
        }

        public static float InSineD(float t, float start = 0f, float end = 1f)
        {
            return (end - start) * 0.5f * Mathf.PI * Mathf.Sin(0.5f * Mathf.PI * t);
        }

        public static float OutSineD(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return Mathf.PI * 0.5f * end * Mathf.Cos(t * (Mathf.PI * 0.5f));
        }

        public static float InOutSineD(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return end * 0.5f * Mathf.PI * Mathf.Sin(Mathf.PI * t);
        }
        public static float InExpoD(float t, float start = 0f, float end = 1f)
        {
            return 10f * NATURAL_LOG_OF_2 * (end - start) * Mathf.Pow(2f, 10f * (t - 1));
        }

        public static float OutExpoD(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            return 5f * NATURAL_LOG_OF_2 * end * Mathf.Pow(2f, 1f - 10f * t);
        }

        public static float InOutExpoD(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;

            if (t < 1)
            {
                return 5f * NATURAL_LOG_OF_2 * end * Mathf.Pow(2f, 10f * (t - 1));
            }

            t--;

            return 5f * NATURAL_LOG_OF_2 * end / Mathf.Pow(2f, 10f * t);
        }

        public static float InCircD(float t, float start = 0f, float end = 1f)
        {
            return (end - start) * t / Mathf.Sqrt(1f - t * t);
        }

        public static float OutCircD(float t, float start = 0f, float end = 1f)
        {
            t--;
            end -= start;
            return -end * t / Mathf.Sqrt(1f - t * t);
        }

        public static float InOutCircD(float t, float start = 0f, float end = 1f)
        {
            t *= 2f;
            end -= start;

            if (t < 1)
            {
                return end * t / (2f * Mathf.Sqrt(1f - t * t));
            }

            t -= 2;

            return -end * t / (2f * Mathf.Sqrt(1f - t * t));
        }

        public static float InBounceD(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            float d = 1f;

            return OutBounceD(0, end, d - t);
        }

        public static float OutBounceD(float t, float start = 0f, float end = 1f)
        {
            t /= 1f;
            end -= start;

            if (t < (1 / 2.75f))
            {
                return 2f * end * 7.5625f * t;
            }
            else if (t < (2 / 2.75f))
            {
                t -= 1.5f / 2.75f;
                return 2f * end * 7.5625f * t;
            }
            else if (t < (2.5 / 2.75f))
            {
                t -= 2.25f / 2.75f;
                return 2f * end * 7.5625f * t;
            }
            else
            {
                t -= 2.625f / 2.75f;
                return 2f * end * 7.5625f * t;
            }
        }

        public static float InOutBounceD(float t, float start = 0f, float end = 1f)
        {
            end -= start;
            float d = 1f;

            if (t < d * 0.5f)
            {
                return InBounceD(0, end, t * 2) * 0.5f;
            }
            else
            {
                return OutBounceD(0, end, t * 2 - d) * 0.5f;
            }
        }

        public static float InBackD(float t, float start = 0f, float end = 1f)
        {
            float s = 1.70158f;

            return 3f * (s + 1f) * (end - start) * t * t - 2f * s * (end - start) * t;
        }

        public static float OutBackD(float t, float start = 0f, float end = 1f)
        {
            float s = 1.70158f;
            end -= start;
            t--;

            return end * ((s + 1f) * t * t + 2f * t * ((s + 1f) * t + s));
        }

        public static float InOutBackD(float t, float start = 0f, float end = 1f)
        {
            float s = 1.70158f;
            end -= start;
            t *= 2f;

            if (t < 1)
            {
                s *= 1.525f;
                return 0.5f * end * (s + 1) * t * t + end * t * ((s + 1f) * t - s);
            }

            t -= 2;
            s *= 1.525f;
            return 0.5f * end * ((s + 1) * t * t + 2f * t * ((s + 1f) * t + s));
        }

        public static float InElasticD(float t, float start = 0f, float end = 1f)
        {
            return OutElasticD(start, end, 1f - t);
        }

        public static float OutElasticD(float t, float start = 0f, float end = 1f)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return a * Mathf.PI * d * Mathf.Pow(2f, 1f - 10f * t) *
                Mathf.Cos(2f * Mathf.PI * (d * t - s) / p) / p - 5f * NATURAL_LOG_OF_2 * a *
                Mathf.Pow(2f, 1f - 10f * t) * Mathf.Sin(2f * Mathf.PI * (d * t - s) / p);
        }

        public static float InOutElasticD(float t, float start = 0f, float end = 1f)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (t < 1)
            {
                t -= 1;

                return -5f * NATURAL_LOG_OF_2 * a * Mathf.Pow(2f, 10f * t) * Mathf.Sin(2 * Mathf.PI * (d * t - 2f) / p) -
                    a * Mathf.PI * d * Mathf.Pow(2f, 10f * t) * Mathf.Cos(2 * Mathf.PI * (d * t - s) / p) / p;
            }

            t -= 1;

            return a * Mathf.PI * d * Mathf.Cos(2f * Mathf.PI * (d * t - s) / p) / (p * Mathf.Pow(2f, 10f * t)) -
                5f * NATURAL_LOG_OF_2 * a * Mathf.Sin(2f * Mathf.PI * (d * t - s) / p) / Mathf.Pow(2f, 10f * t);
        }

        public static float SpringD(float t, float start = 0f, float end = 1f)
        {
            t = Mathf.Clamp01(t);
            end -= start;

            // Damn... Thanks http://www.derivative-calculator.net/
            // TODO: And it's a little bit wrong
            return end * (6f * (1f - t) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - t, 1.2f) *
                Mathf.Sin(Mathf.PI * t * (2.5f * t * t * t + 0.2f)) + Mathf.Pow(1f - t, 2.2f) *
                (Mathf.PI * (2.5f * t * t * t + 0.2f) + 7.5f * Mathf.PI * t * t * t) *
                Mathf.Cos(Mathf.PI * t * (2.5f * t * t * t + 0.2f)) + 1f) -
                6f * end * (Mathf.Pow(1 - t, 2.2f) * Mathf.Sin(Mathf.PI * t * (2.5f * t * t * t + 0.2f)) + t
                / 5f);

        }

        public delegate float Function(float t, float s, float e);

        /// <summary>
        /// Returns the function associated to the easingFunction enum. This t returned should be cached as it allocates memory
        /// to return.
        /// </summary>
        /// <param name="easingFunction">The enum associated with the easing function.</param>
        /// <returns>The easing function</returns>
        public static Function GetEasingFunction(Ease easingFunction)
        {
            return easingFunction switch
            {
                Ease.InQuad => InQuad,
                Ease.OutQuad => OutQuad,
                Ease.InOutQuad => InOutQuad,
                Ease.InCubic => InCubic,
                Ease.OutCubic => OutCubic,
                Ease.InOutCubic => InOutCubic,
                Ease.InQuart => InQuart,
                Ease.OutQuart => OutQuart,
                Ease.InOutQuart => InOutQuart,
                Ease.InQuint => InQuint,
                Ease.OutQuint => OutQuint,
                Ease.InOutQuint => InOutQuint,
                Ease.InSine => InSine,
                Ease.OutSine => OutSine,
                Ease.InOutSine => InOutSine,
                Ease.InExpo => InExpo,
                Ease.OutExpo => OutExpo,
                Ease.InOutExpo => InOutExpo,
                Ease.InCirc => InCirc,
                Ease.OutCirc => OutCirc,
                Ease.InOutCirc => InOutCirc,
                Ease.Linear => Linear,
                Ease.Spring => Spring,
                Ease.InBounce => InBounce,
                Ease.OutBounce => OutBounce,
                Ease.InOutBounce => InOutBounce,
                Ease.InBack => InBack,
                Ease.OutBack => OutBack,
                Ease.InOutBack => InOutBack,
                Ease.InElastic => InElastic,
                Ease.OutElastic => OutElastic,
                Ease.InOutElastic => InOutElastic,
                _ => null,
            };
        }

        /// <summary>
        /// Gets the derivative function of the appropriate easing function. If you use an easing function for position then this
        /// function can get you the speed at a given time (normalized).
        /// </summary>
        /// <param name="easingFunction"></param>
        /// <returns>The derivative function</returns>
        public static Function GetEasingFunctionDerivative(Ease easingFunction)
        {
            return easingFunction switch
            {
                Ease.InQuad => InQuadD,
                Ease.OutQuad => OutQuadD,
                Ease.InOutQuad => InOutQuadD,
                Ease.InCubic => InCubicD,
                Ease.OutCubic => OutCubicD,
                Ease.InOutCubic => InOutCubicD,
                Ease.InQuart => InQuartD,
                Ease.OutQuart => OutQuartD,
                Ease.InOutQuart => InOutQuartD,
                Ease.InQuint => InQuintD,
                Ease.OutQuint => OutQuintD,
                Ease.InOutQuint => InOutQuintD,
                Ease.InSine => InSineD,
                Ease.OutSine => OutSineD,
                Ease.InOutSine => InOutSineD,
                Ease.InExpo => InExpoD,
                Ease.OutExpo => OutExpoD,
                Ease.InOutExpo => InOutExpoD,
                Ease.InCirc => InCircD,
                Ease.OutCirc => OutCircD,
                Ease.InOutCirc => InOutCircD,
                Ease.Linear => LinearD,
                Ease.Spring => SpringD,
                Ease.InBounce => InBounceD,
                Ease.OutBounce => OutBounceD,
                Ease.InOutBounce => InOutBounceD,
                Ease.InBack => InBackD,
                Ease.OutBack => OutBackD,
                Ease.InOutBack => InOutBackD,
                Ease.InElastic => InElasticD,
                Ease.OutElastic => OutElasticD,
                Ease.InOutElastic => InOutElasticD,
                _ => null
            };
        }
    }
}