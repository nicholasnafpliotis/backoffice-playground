using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

// NOTE: This object requires that System.Windows.Forms is referenced in your project.
namespace Backoffice.Services
{
    /// <summary>
    /// Represents a class that can be used to render
    /// a JavaScript file that contains an object that contains resource keys and values
    /// of a specific global resX file dependant on the CurrentUI culture.
    /// </summary>
    public class GlobalResourcesToJavaScriptHelper : Control
    {
        #region Properties
        /// <summary>
        /// The name of the Global ResX file (ex: "Resource1"
        ///       if the ResX file is "Resource1.resx")
        /// </summary>
        public string GlobalResXFileName { get; set; }

        /// <summary>
        /// Sets and Gets the generated JavaScript object name.
        /// if not set it will return the normalized GlobalResXFileName.
        /// </summary>
        public string JavaScriptObjectName
        {
            set { _javaScriptObjectName = value; }
            get
            {
                if(!string.IsNullOrEmpty(_javaScriptObjectName) && _javaScriptObjectName.Trim() != string.Empty) return _javaScriptObjectName;
                else return GlobalResXFileName;
            }
        }
        private string _javaScriptObjectName;
        #endregion

        #region Public Methods
        public string GetResXFilePath()
        {
            return MapPathSecure(Path.Combine("~//App_GlobalResources",
                   GlobalResXFileName + ".resx"));
        }

        public string GetJavaScript()
        {
            if(!FileExists())
            {
                return "alert('GlobalResourcesToJavaScript: Can't find the file " + GetResXFilePath() + ".');";
            }

            if (!string.IsNullOrEmpty(JavaScriptObjectName) && File.Exists(GetResXFilePath()))
            {
                var script = new StringBuilder();
                using (System.Resources.ResXResourceReader resourceReader = new System.Resources.ResXResourceReader(GetResXFilePath()))
                {
                    // Start by namespacing the object
                    // If there are no periods in our name, simply start our object definition
                    if(JavaScriptObjectName.Split('.').Length == 1)
                    {
                        script.Append("var " + JavaScriptObjectName + " = { ");
                    }

                    // If there are periods in the name, the developer wants to nest their objects.
                    // This is helpful if there is more than one resource file being translated into javascript.
                    // An example would be the core resource file, with the name 'resources'.
                    // Then, they translate a second resource file, whit the name 'resources.labels'.
                    // This helps the developer to organize their code accordingly.
                    else
                    {
                        var nameparts = JavaScriptObjectName.Split('.');
                        for(var x = 0; x < nameparts.Length; x++)
                        {
                            // If this is the first part of the namespace, let's any existing instances, 
                            // or create a new one if this is the first time it's been instantiated.
                            if(x == 0)
                            {
                                script.Append("var " + nameparts[0] + " = " + nameparts[0] + " || {};\r");
                            }

                            // If this is not the first part...
                            else
                            {
                                // Create the namespace name up to this point.
                                var variable = "";
                                for(var y = 0; y < x + 1; y++)
                                {
                                    if(variable != "") variable += ".";
                                    variable += nameparts[y];
                                }

                                // If this is the last part of the namespace, let's start defining the object.
                                if(x == JavaScriptObjectName.Split('.').Length - 1)
                                {
                                    script.Append(variable + " = { ");
                                }

                                // If there are more parts of the namespace to define, let's set the object and move on.
                                else
                                {
                                    script.Append(variable + " = {};\r");
                                }
                            }
                        }
                    }


                    // Write out the resource items into JSON
                    bool first = true;
                    foreach (DictionaryEntry entry in resourceReader)
                    {
                        if (first)
                            first = false;
                        else
                            script.Append(" , ");
 
                        script.Append(NormalizeVariableName(entry.Key as string));
                        script.Append(":");
                        script.Append("'" + GetResourceValue(entry.Key as string) + "'");
                    }
                    script.Append(" };");
                    return script.ToString();
                }
            }

            return "alert('GlobalResourcesToJavaScript: Could not load the resource file " + GetResXFilePath() + ".');";
        }
        #endregion

        #region Helper Methods
        public bool FileExists()
        {
            if (!File.Exists(GetResXFilePath())) return false;
            else return true;
        }

        /// <summary>
        /// Normalizes the variable names to be used as JavaScript variable names
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected static string NormalizeVariableName(string key)
        {
            key = key.Replace('.', '_');
            key = key.Replace(' ', '_');

            return key;
        }

        protected string GetResourceValue(string key)
        {
            string value = HttpContext.GetGlobalResourceObject(GlobalResXFileName, key) as string;

            // Account for semi-colons
            value = value.Replace("'", "\\'");

            return value == null ? string.Empty : value;
        }
        #endregion
    }
}