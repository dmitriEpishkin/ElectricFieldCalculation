﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.35317
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nordwest.Wpf.Controls.Properties {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Nordwest.Wpf.Controls.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на y &lt; yMin || y &gt; yMax.
        /// </summary>
        internal static string ErrorData_ArgumentException {
            get {
                return ResourceManager.GetString("ErrorData_ArgumentException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Only Linear or Log10 Axis.
        /// </summary>
        internal static string GridElement_GetValue_AxisTypeNotSupported_Exception {
            get {
                return ResourceManager.GetString("GridElement_GetValue_AxisTypeNotSupported_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Ex Channel Number.
        /// </summary>
        internal static string LayoutCorrectionView_ExChannelNumber_Text {
            get {
                return ResourceManager.GetString("LayoutCorrectionView_ExChannelNumber_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на values.Count != colors.Count.
        /// </summary>
        internal static string Palette_SetPalette_NotSameCount_ArgumentException {
            get {
                return ResourceManager.GetString("Palette_SetPalette_NotSameCount_ArgumentException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на start&gt;end.
        /// </summary>
        internal static string TableChartCell_StartGreaterThenEnd_Exception {
            get {
                return ResourceManager.GetString("TableChartCell_StartGreaterThenEnd_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на position &gt; end.
        /// </summary>
        internal static string TableChartGroup_PositionGraterThenEnd_Exception {
            get {
                return ResourceManager.GetString("TableChartGroup_PositionGraterThenEnd_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на start &gt; position.
        /// </summary>
        internal static string TableChartGroup_StartGreaterThenPosition_Exception {
            get {
                return ResourceManager.GetString("TableChartGroup_StartGreaterThenPosition_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на key != value.GetType().
        /// </summary>
        internal static string ToolCollection_Add_WrongTypeArgumentException {
            get {
                return ResourceManager.GetString("ToolCollection_Add_WrongTypeArgumentException", resourceCulture);
            }
        }
    }
}
