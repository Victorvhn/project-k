﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectK.Core.Resource {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("ProjectK.Core.Resource.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string AmountType_Fixed {
            get {
                return ResourceManager.GetString("AmountType_Fixed", resourceCulture);
            }
        }
        
        public static string AmountType_Variable {
            get {
                return ResourceManager.GetString("AmountType_Variable", resourceCulture);
            }
        }
        
        public static string CannotDeleteUser {
            get {
                return ResourceManager.GetString("CannotDeleteUser", resourceCulture);
            }
        }
        
        public static string CanNotUpdateStartsAt {
            get {
                return ResourceManager.GetString("CanNotUpdateStartsAt", resourceCulture);
            }
        }
        
        public static string CategoryAlreadyExists {
            get {
                return ResourceManager.GetString("CategoryAlreadyExists", resourceCulture);
            }
        }
        
        public static string CategoryNameNotFound {
            get {
                return ResourceManager.GetString("CategoryNameNotFound", resourceCulture);
            }
        }
        
        public static string CategoryNotFound {
            get {
                return ResourceManager.GetString("CategoryNotFound", resourceCulture);
            }
        }
        
        public static string DateFilterNotProvided {
            get {
                return ResourceManager.GetString("DateFilterNotProvided", resourceCulture);
            }
        }
        
        public static string EmailAlreadyInUse {
            get {
                return ResourceManager.GetString("EmailAlreadyInUse", resourceCulture);
            }
        }
        
        public static string InvalidActionTypeProvided {
            get {
                return ResourceManager.GetString("InvalidActionTypeProvided", resourceCulture);
            }
        }
        
        public static string InvalidTransactionTypeProvided {
            get {
                return ResourceManager.GetString("InvalidTransactionTypeProvided", resourceCulture);
            }
        }
        
        public static string Overpaid {
            get {
                return ResourceManager.GetString("Overpaid", resourceCulture);
            }
        }
        
        public static string PlannedTransactionNotFound {
            get {
                return ResourceManager.GetString("PlannedTransactionNotFound", resourceCulture);
            }
        }
        
        public static string Recurrence_Annual {
            get {
                return ResourceManager.GetString("Recurrence_Annual", resourceCulture);
            }
        }
        
        public static string Recurrence_Monthly {
            get {
                return ResourceManager.GetString("Recurrence_Monthly", resourceCulture);
            }
        }
        
        public static string TransactionNotFound {
            get {
                return ResourceManager.GetString("TransactionNotFound", resourceCulture);
            }
        }
        
        public static string TransactionType_Expense {
            get {
                return ResourceManager.GetString("TransactionType_Expense", resourceCulture);
            }
        }
        
        public static string TransactionType_Income {
            get {
                return ResourceManager.GetString("TransactionType_Income", resourceCulture);
            }
        }
        
        public static string UnableToCreatePlannedTransaction {
            get {
                return ResourceManager.GetString("UnableToCreatePlannedTransaction", resourceCulture);
            }
        }
        
        public static string Underpaid {
            get {
                return ResourceManager.GetString("Underpaid", resourceCulture);
            }
        }
        
        public static string UserNotFound {
            get {
                return ResourceManager.GetString("UserNotFound", resourceCulture);
            }
        }
        
        public static string UserWithEmailNotFound {
            get {
                return ResourceManager.GetString("UserWithEmailNotFound", resourceCulture);
            }
        }
        
        public static string UnableToCreateTransaction {
            get {
                return ResourceManager.GetString("UnableToCreateTransaction", resourceCulture);
            }
        }
        
        public static string DefaultCategory {
            get {
                return ResourceManager.GetString("DefaultCategory", resourceCulture);
            }
        }
        
        public static string CustomPlannedTransaction {
            get {
                return ResourceManager.GetString("CustomPlannedTransaction", resourceCulture);
            }
        }
    }
}
