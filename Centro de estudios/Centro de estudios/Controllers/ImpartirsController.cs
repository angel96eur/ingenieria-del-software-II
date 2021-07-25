using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Centro_de_estudios.Data;
using Centro_de_estudios.Models;
using Microsoft.AspNetCore.Authorization;
using Centro_de_estudios.Models.ImpartirViewModel;

namespace Centro_de_estudios.Controllers
{
    [Authorize(Roles = "Profesor")]
    public class ImpartirsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImpartirsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Impartirs
        public async Task<IActionResult> Index()
        {

            return View(_context.Impartir.Include(p=>p.Profesor).Where(p=>p.Profesor.UserName.Equals(User.Identity.Name)).OrderByDescending(p=>p.fecha).ToList());
        }


        // GET: Impartirs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impartir = _context.Impartir
                .Include(p => p.ImpartirAsignaturas).ThenInclude<Impartir, ImpartirAsignatura, Asignatura>(p => p.Asignatura).Include(p => p.Profesor).Where(p => p.ImpartirId == id).ToList();
            if (impartir.Count == 0)
            {
                return NotFound();
            }

            return View(impartir.First());
        }

        // GET: Impartirs/Create
        public IActionResult Create(SelectedAsignaturasForImpartirViewModel selectedAsignaturas)
        {
            Asignatura asignatura;
            int id;

            ImpartirCreateViewModel impartir = new ImpartirCreateViewModel();
            impartir.ImpartirAsignaturas = new List<ImpartirAsignatura>();

            if(selectedAsignaturas.IdsToAdd == null)
            {
                ModelState.AddModelError("AsignaturaNoSelected", "Deberías seleccionar una asignatura para ser impartida, por favor");
            } else 
                foreach (string ids in selectedAsignaturas.IdsToAdd)
                {
                    id = int.Parse(ids);
                    asignatura = _context.Asignatura.Include(m => m.Intensificacion).FirstOrDefault<Asignatura>(m => m.AsignaturaID.Equals(id));
                    impartir.ImpartirAsignaturas.Add(new ImpartirAsignatura() { cantidadAsignatura = 1, Asignatura=asignatura});
                }

            Profesor profesor = _context.Users.OfType<Profesor>().FirstOrDefault<Profesor>(u => u.UserName.Equals(User.Identity.Name));
            impartir.Nombre = profesor.Nombre;
            impartir.PrimerApellido = profesor.PrimerApellido;
            impartir.SegundoApellido = profesor.SegundoApellido;

            return View(impartir);
        }



        // POST: Impartirs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(ImpartirCreateViewModel impartirViewModel, IList<ImpartirAsignatura> impartirAsignaturas)
        {
            Asignatura asignatura;
            Profesor profesor;
            Impartir impartir = new Impartir();
            impartir.mesesDocenciaTotal = 0;
            impartir.ImpartirAsignaturas = new List<ImpartirAsignatura>();
            ModelState.Clear();

            foreach (ImpartirAsignatura item in impartirAsignaturas)
            {
                asignatura = await _context.Asignatura.FirstOrDefaultAsync<Asignatura>(m=>m.AsignaturaID == item.Asignatura.AsignaturaID);
                if(asignatura.MinimoMesesDocencia > item.cantidadAsignatura)
                {
                    ModelState.AddModelError("", $"You should select at least a Asignatura to be purchased, please");

                } else
                {
                    if (item.cantidadAsignatura > 0)
                    {
                        
                        item.Asignatura = asignatura;
                        item.Impartir = impartir;
                       
                        impartir.mesesDocenciaTotal += item.cantidadAsignatura * asignatura.MinimoMesesDocencia;
                        impartir.ImpartirAsignaturas.Add(item);
                    }
                }
            }
            profesor = await _context.Users.OfType<Profesor>().FirstOrDefaultAsync<Profesor>(u => u.UserName.Equals(User.Identity.Name));

            if (ModelState.ErrorCount > 0)
            {
                impartirViewModel.Nombre = profesor.Nombre;
                impartirViewModel.PrimerApellido = profesor.PrimerApellido;
                impartirViewModel.SegundoApellido = profesor.SegundoApellido;
                return View(impartirViewModel);
            }

            if(impartir.mesesDocenciaTotal == 0)
            {
                impartirViewModel.Nombre = profesor.Nombre;
                impartirViewModel.PrimerApellido = profesor.PrimerApellido;
                impartirViewModel.SegundoApellido = profesor.SegundoApellido;
                ModelState.AddModelError("AsignaturaForImpartir0",$"Por favor indique una asignatura");
                impartirViewModel.ImpartirAsignaturas = impartirAsignaturas;
                return View(impartirViewModel);
            }

            impartir.Profesor = profesor;
            impartir.fecha = DateTime.Now;
            _context.Add(impartir);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = impartir.ImpartirId });
        }

        // GET: Impartirs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impartir = await _context.Impartir.Include(p=>p.ImpartirAsignaturas).ThenInclude<Impartir, ImpartirAsignatura, Asignatura>(i => i.Asignatura).Include(p=>p.Profesor).SingleOrDefaultAsync(m=>m.ImpartirId==id);
            if (impartir == null)
            {
                return NotFound();
            }
            ViewData["ProfesorId"] = new SelectList(_context.Profesor, "Id", "Id", impartir.ProfesorId);
            return View(impartir);
        }

        // POST: Impartirs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImpartirId,ProfesorId,mesesDocenciaTotal,fecha")] Impartir impartir)
        {
            if (id != impartir.ImpartirId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(impartir);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImpartirExists(impartir.ImpartirId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ProfesorId"] = new SelectList(_context.Set<Profesor>(), "Id", "Id", impartir.ProfesorId);
            return View(impartir);
        }

        // GET: Impartirs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impartir = await _context.Impartir
                .SingleOrDefaultAsync(m => m.ImpartirId == id);
            if (impartir == null)
            {
                return NotFound();
            }

            return View(impartir);
        }

        // POST: Impartirs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var impartir = await _context.Impartir.Include(p=>p.ImpartirAsignaturas).SingleOrDefaultAsync(m=>m.ImpartirId == id);
            _context.Impartir.Remove(impartir);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ImpartirExists(int id)
        {
            return _context.Impartir.Any(e => e.ImpartirId == id);
        }
        
        //GET: Impartirs/SelectAsignaturasForImpartir
        public IActionResult SelectAsignaturasForImpartir(string asignaturaNombre, string asignaturaIntensificacionSelected)
        {
            SelectAsignaturasForImpartirViewModel selectAsignaturas = new SelectAsignaturasForImpartirViewModel();
            selectAsignaturas.Intensificaciones = new SelectList(_context.Intensificacion.Select(g => g.NombreIntensificacion).ToList());
            selectAsignaturas.Asignaturas = _context.Asignatura.Include(m => m.Intensificacion).Where(m => m.MinimoMesesDocencia > 0);
            if (asignaturaNombre != null)
            {
                selectAsignaturas.Asignaturas = selectAsignaturas.Asignaturas.Where(m => m.NombreAsignatura.Contains(asignaturaNombre));
            }
            if (asignaturaIntensificacionSelected != null)
            {
                selectAsignaturas.Asignaturas = selectAsignaturas.Asignaturas.Where(m => m.Intensificacion.NombreIntensificacion.Contains(asignaturaIntensificacionSelected));
            }
            selectAsignaturas.Asignaturas.ToList();
            return View(selectAsignaturas);
        }

        // POST: Impartirs/SelectAsignaturasForCompartir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectAsignaturasForImpartir(SelectedAsignaturasForImpartirViewModel selectedAsignaturas)
        {
            if (selectedAsignaturas.IdsToAdd!=null)
            {
                return RedirectToAction("Create", selectedAsignaturas);

            }

            //error que se muestra si no hay asignaturas seleccionadas
            ModelState.AddModelError(string.Empty, "Debes de seleccionar al menos una asignatura");
            SelectAsignaturasForImpartirViewModel selectAsignaturas = new SelectAsignaturasForImpartirViewModel();
            selectAsignaturas.Intensificaciones = new SelectList(_context.Intensificacion.Select(g => g.NombreIntensificacion).ToList());
            selectAsignaturas.Asignaturas = _context.Asignatura.Include(m => m.Intensificacion).Where(m => m.MinimoMesesDocencia > 0).ToList();
            return View(selectAsignaturas);
        }
    }
}
