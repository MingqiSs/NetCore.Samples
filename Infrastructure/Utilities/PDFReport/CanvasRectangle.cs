using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Utilities.PDFReport
{
        /// <summary>
        /// 画布对象
        /// </summary>
        public class CanvasRectangle
        {
            #region CanvasRectangle属性
            public float StartX { get; set; }
            public float StartY { get; set; }
            public float RectWidth { get; set; }
            public float RectHeight { get; set; }
            #endregion

            #region 初始化Rectangle
            /// <summary>
            /// 提供rectangle信息
            /// </summary>
            /// <param name="startX">起点X坐标</param>
            /// <param name="startY">起点Y坐标</param>
            /// <param name="rectWidth">指定rectangle宽</param>
            /// <param name="rectHeight">指定rectangle高</param>
            public CanvasRectangle(float startX, float startY, float rectWidth, float rectHeight)
            {
                this.StartX = startX;
                this.StartY = startY;
                this.RectWidth = rectWidth;
                this.RectHeight = rectHeight;
            }

            #endregion

            #region 获取图形缩放百分比
            /// <summary>
            /// 获取指定宽高压缩后的百分比
            /// </summary>
            /// <param name="width">目标宽</param>
            /// <param name="height">目标高</param>
            /// <param name="containerRect">原始对象</param>
            /// <returns>目标与原始对象百分比</returns>
            public static float GetPercentage(float width, float height, CanvasRectangle containerRect)
            {
                float percentage = 0;

                if (height > width)
                {
                    percentage = containerRect.RectHeight / height;

                    if (width * percentage > containerRect.RectWidth)
                    {
                        percentage = containerRect.RectWidth / width;
                    }
                }
                else
                {
                    percentage = containerRect.RectWidth / width;

                    if (height * percentage > containerRect.RectHeight)
                    {
                        percentage = containerRect.RectHeight / height;
                    }
                }

                return percentage;
            }
            #endregion

        }
}
