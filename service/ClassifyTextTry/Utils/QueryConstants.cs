using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassifyTextTry.Utils
{
    public class QueryConstants
    {
        public const string AllCategory = "g.V().hasLabel('category')";

        public const string AllCategoryLinks0 = "g.V('{0}').out('link')";
        public const string AllCategoryLinks1 = "g.V('{0}').out('subcatgetory').out('link')";
        public const string AllCategoryLinks2 = "g.V('{0}').out('subcatgetory').out('subcatgetory').out('link')";
        public const string AllCategoryLinks3 = "g.V('{0}').out('subcatgetory').out('subcatgetory').out('subcatgetory').out('link')";
        public const string AllSubCategory = "g.V('{0}').out('subcatgetory')";
        public const string AllSubCategoryLinks1 = "g.V('{0}').out('link')";
        public const string AllSubCategoryLinks2 = "g.V('{0}')out('subcatgetory').out('link')";


        public const string AddVertexCategory = "g.addV('category').property('id', '{0}')";
        public const string AddVertexSubcategory = "g.addV('category2').property('id', '{0}')";
        public const string AddVertexLinkdata = "g.addV('linkdata').property('id', '{0}').property('data', '{1}')";
        public const string AddEdgeCategoryToSubcategory = "g.V('{0}').addE('subcatgetory').to(g.V('{1}'))";
        public const string AddEdgeSubOrCategoryToLinkdata = "g.V('{0}').addE('link').to(g.V('{1}'))";

        public const string GetVertex = "g.V('{0}')";

        /* Example
         * 
        private const string AllCategory = "g.V().hasLabel('category')";
        private const string AllCategoryLinks1 = "g.V('Travel').out('subcatgetory').out('link')";
        private const string AllCategoryLinks2 = "g.V('Travel').out('subcatgetory').out('subcatgetory').out('link')";
        private const string AllCategoryLinks3 = "g.V('Travel').out('subcatgetory').out('subcatgetory').out('subcatgetory').out('link')";
        private const string AllSubCategory = "g.V('Travel').out('subcatgetory')";
        private const string AllSubCategoryLinks1 = "g.V('Tourist Destinations').out('link')";
        private const string AllSubCategoryLinks2 = "g.V('Tourist Destinations')out('subcatgetory').out('link')";
        */
    }
}
