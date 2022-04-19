using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalRound
{
    [Transaction(TransactionMode.Manual)]
    public class FinalRoundCreateRoom : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;



            CreateRooms(doc);


            return Result.Succeeded;
        }
        private void CreateRooms(Document doc)
        {
            PhaseArray phases = doc.Phases;
            Phase createRoomsInPhase = phases.get_Item(phases.Size - 1);
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Level));
            List<Level> listLevel = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .OfType<Level>()
                .ToList();


            Level level1 = listLevel
                .Where(x => x.Name.Equals("Уровень 1"))
                .FirstOrDefault();

            Level level2 = listLevel
                .Where(x => x.Name.Equals("Уровень 2"))
                .FirstOrDefault();

            Level level3 = listLevel
                .Where(x => x.Name.Equals("Уровень 3"))
                .FirstOrDefault();

            Level level4 = listLevel
                .Where(x => x.Name.Equals("Уровень 4"))
                .FirstOrDefault();

            using (Transaction tran = new Transaction(doc))
            {
                int x = 1;
                tran.Start("tran1");
                foreach (Level level in listLevel)
                {
                    PlanTopology topology = doc.get_PlanTopology(level, createRoomsInPhase);
                    PlanCircuitSet circuitSet = topology.Circuits;
                    foreach (PlanCircuit circuit in circuitSet)
                    {
                        if (!circuit.IsRoomLocated && level == level1)
                        {
                            Room room = doc.Create.NewRoom(null, circuit);
                            room.Name = "1_" + x + "-первый этаж," + x + "-е помещение";
                            x++;
                        }
                        if (!circuit.IsRoomLocated && level == level2)
                        {
                            Room room = doc.Create.NewRoom(null, circuit);

                            room.Name = "2_" + x + "-второй этаж," + x + "-е помещение";
                            x++;
                        }
                        if (!circuit.IsRoomLocated && level == level3)
                        {
                            Room room = doc.Create.NewRoom(null, circuit);

                            room.Name = "3_" + x + "-третий этаж," + x + "-е помещение";
                            x++;
                        }
                        if (!circuit.IsRoomLocated && level == level4)
                        {
                            Room room = doc.Create.NewRoom(null, circuit);

                            room.Name = "4_" + x + "-четвертый этаж," + x + "-е помещение";
                            x++;
                        }
                    }
                }


                tran.Commit();
            }
        }


    }
}
