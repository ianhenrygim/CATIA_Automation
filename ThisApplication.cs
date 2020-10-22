using System;
using System.Text;
using PLMModelerBaseIDL;
using VPMEditorContextIDL;
using ProductStructureClientIDL;
using INFITF;
using PARTITF;
using MECMOD;
using FCBITF;
using PLMAccessIDLItf;
    
namespace Macro_library_VSTA00000081_vst_R1132100327574_00000081_A_1
{
  public sealed partial class ThisApplication : ObjectModelAgentLib.CATIAEntryPoint
  {

        public static void M(string content)
        {
            CATIA.MsgBox(content, 0, "Test", "", 0);
        }

        // Naviation Recursion
        public static void NavigateProduct(PLMEntity vref, ref StringBuilder stb)
        {
            try
            {
                VPMReference vpmref = (VPMReference)vref;

                for (int i = 1; i < vpmref.Instances.Count + 1; i++)
                {
                    stb.AppendLine(vpmref.Instances.Item(i).get_Name());
                    NavigateProduct(((VPMInstance)vpmref.Instances.Item(i)).ReferenceInstanceOf, ref stb);
                }

                for (int i = 1; i < vpmref.RepInstances.Count + 1; i++)
                {
                    stb.AppendLine(vpmref.RepInstances.Item(i).get_Name());
                    NavigateProduct(((VPMRepInstance)vpmref.Instances.Item(i)).ReferenceInstanceOf, ref stb);
                }
            }
            catch (Exception)
            {

            }
        }

        public static void CATMain()
        {
            SearchService a = (SearchService) CATIA.GetSessionService("Search");
            DatabaseSearch b = a.DatabaseSearch;

            //b.set_BaseType("VPMReference");
            //b.AddEasyCriteria("PLM_ExternalID", "Ship");

            //PLMEntities result = b.Results;


            StringBuilder stb = new StringBuilder();

            //stb.AppendLine(String.Format("Database Search : {0}", b.SearchCount.ToString()));

            VPMRootOccurrence occ = (VPMRootOccurrence) CATIA.ActiveEditor.ActiveObject;
            stb.AppendLine(occ.get_Name());
            VPMReference rootRef = occ.ReferenceRootOccurrenceOf;
            stb.AppendLine(rootRef.get_Name());
            stb.AppendLine(String.Format("Instances {0}",rootRef.Instances.Count));
            stb.AppendLine(String.Format("RepInstances {0}", rootRef.RepInstances.Count));

            for (int i = 1; i < rootRef.Instances.Count + 1; i++)
            {
                VPMInstance vi = (VPMInstance) rootRef.Instances.Item(i);
                stb.AppendLine(String.Format("+-- Instance {0}", vi.get_Name()));

                VPMReference vr = vi.ReferenceInstanceOf;
                stb.AppendLine(String.Format("+-- Instances {0}", vr.Instances.Count));
                stb.AppendLine(String.Format("+-- RepInstanes {0}", vr.RepInstances.Count));

                for (int j = 1; j < vr.RepInstances.Count + 1; j++)
                {
                    VPMRepInstance vri = (VPMRepInstance) vr.RepInstances.Item(j);
                    stb.AppendLine(String.Format("   +-- RepInstance {0}", vri.get_Name()));
                    stb.AppendLine(String.Format("   +-- RepInstance {0}", vri.ReferenceInstanceOf.get_Name()));
                    Part vpart = (Part) vri.ReferenceInstanceOf.GetItem("Part");
                    stb.AppendLine(String.Format("      +-- Part {0}", vpart.get_Name()));
                    stb.AppendLine(String.Format("      +-- GeometricElem {0}", vpart.GeometricElements.Count));
                    stb.AppendLine(String.Format("      +-- GeometricElem {0}", vpart.HybridBodies.Count));

                    HybridBodies hb = vpart.HybridBodies;
                    for (int k = 1; k < hb.Count + 1; k++)
                    {
                        stb.AppendLine(String.Format("         +-- HybridBody {0}", hb.Item(k).get_Name()));
                    }
                }
            }

            VPMRepInstances vris = rootRef.RepInstances;
            for (int i = 1; i < rootRef.RepInstances.Count + 1; i++)
            {
                VPMRepInstance vri = (VPMRepInstance) vris.Item(i);
                stb.AppendLine(String.Format("   +-- RepInstance {0}", vri.get_Name()));
                Part vpart = (Part) vri.ReferenceInstanceOf.GetItem("Part");
                stb.AppendLine(String.Format("   +-- Part {0}", vpart.get_Name()));
                stb.AppendLine(String.Format("      +-- GeometricElem {0}", vpart.GeometricElements.Count));
                stb.AppendLine(String.Format("      +-- GeometricElem {0}", vpart.HybridBodies.Count));
            }

            M(stb.ToString());
        }
    }
}
