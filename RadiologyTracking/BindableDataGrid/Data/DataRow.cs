using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Threading;
using System.Collections.ObjectModel;

namespace BindableDataGrid.Data
{
    /// <summary>
    /// Represents a row of data in a DataTable
    /// </summary>
    public class DataRow
    {
        #region "Properties"

        /// <summary>
        /// Collection of items (cells) in a row
        /// </summary>
        public Dictionary<string, object> Items { get; set; }

        /// <summary>
        /// Property indexer to access the items collection by key
        /// </summary>
        /// <param name="key">Key (column name)</param>
        /// <returns>Value of the corresponding cell</returns>
        public object this[string key]
        {
            get
            {
                return Items[key];
            }
            set
            {
                Items[key] = value;
            }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataRow()
        {
            this.Items = new Dictionary<string, object>();
        }

        /// <summary>
        /// Specifies equality between objects
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True or False</returns>
        public new bool Equals(object obj)
        {
            bool equal = false;
            if (obj is DataRow)
            {
                DataRow dest = obj as DataRow;
                if (this.Items.Count == dest.Items.Count)
                {
                    equal = true;
                    foreach (string key in this.Items.Keys)
                    {
                        equal = equal && dest.Items.ContainsKey(key) && this.Items[key].Equals(dest.Items[key]);
                    }
                }
            }
            return equal;
        }

        /// <summary>
        /// Generates an assembly that can be used to create instances of a dynamic class (row)
        /// </summary>
        /// <returns>An assembly that contains a class defining properties for the row</returns>
        public Assembly EmitAssembly()
        {
            // Create an assembly
            AssemblyName myAssemblyName = new AssemblyName();
            myAssemblyName.Name = "DataRowAssembly";
            AssemblyBuilder myAssembly = Thread.GetDomain().DefineDynamicAssembly(myAssemblyName, AssemblyBuilderAccess.Run);

            // Create a module
            ModuleBuilder myModule = myAssembly.DefineDynamicModule("DataRowModule", true);

            // Define a public class
            TypeBuilder myTypeBuilder = myModule.DefineType("DataRowObject", TypeAttributes.Public | TypeAttributes.Class);

            // Define private fields based on the row items
            foreach (string key in this.Items.Keys)
            {
                this.BuildFieldAndProperty(key, (this.Items[key]).GetType(), myTypeBuilder);
            }

            // Create the class. Even though we don't use it, we need to create the type
            Type myType1 = myTypeBuilder.CreateType();

            return myAssembly;
        }

        /// <summary>
        /// Creates IL for a property in the class, as defined by the parameters of the cell
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="type">Type of the property</param>
        /// <param name="myTypeBuilder"></param>
        private void BuildFieldAndProperty(string name, Type type, TypeBuilder myTypeBuilder)
        {
            // Define the field
            FieldBuilder myFieldBuilder = myTypeBuilder.DefineField(name, type, FieldAttributes.Private);

            // The last argument of DefineProperty is null, because the
            // property has no parameters. (If you don't specify null, you must
            // specify an array of Type objects. For a parameterless property,
            // use an array with no elements: new Type[] {})
            PropertyBuilder myPropertyBuilder = myTypeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, null);

            // The property set and property get methods require a special set of attributes
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the "get" accessor method for the property
            MethodBuilder getMethodBuilder = myTypeBuilder.DefineMethod("get_" + name, getSetAttr, type, Type.EmptyTypes);
            ILGenerator custNameGetIL = getMethodBuilder.GetILGenerator();
            custNameGetIL.Emit(OpCodes.Ldarg_0);
            custNameGetIL.Emit(OpCodes.Ldfld, myFieldBuilder);
            custNameGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method for the property
            MethodBuilder setMethodBuilder = myTypeBuilder.DefineMethod("set_" + name, getSetAttr, null, new Type[] { type });
            ILGenerator custNameSetIL = setMethodBuilder.GetILGenerator();
            custNameSetIL.Emit(OpCodes.Ldarg_0);
            custNameSetIL.Emit(OpCodes.Ldarg_1);
            custNameSetIL.Emit(OpCodes.Stfld, myFieldBuilder);
            custNameSetIL.Emit(OpCodes.Ret);

            // Last, we must map the two methods created above to our PropertyBuilder to
            // their corresponding behaviors, "get" and "set" respectively
            myPropertyBuilder.SetGetMethod(getMethodBuilder);
            myPropertyBuilder.SetSetMethod(setMethodBuilder);
        }

        #endregion
    }
}