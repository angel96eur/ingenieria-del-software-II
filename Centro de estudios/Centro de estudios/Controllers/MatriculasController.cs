using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Centro_de_estudios.Data;
using Centro_de_estudios.Models;
using Centro_de_estudios.Models.MatriculaViewModels;
using Microsoft.AspNetCore.Authorization;


namespace Centro_de_estudios.Controllers
{
    [Authorize(Roles = "Estudiante")]
    public class MatriculasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MatriculasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Matriculas
        public async Task<IActionResult> Index()
        {
            return View(_context.Matricula.Include(p => p.Estudiante).Where(p => p.Estudiante.UserName.Equals(User.Identity.Name)).OrderByDescending(p => p.FechaMatricula).ToList());
        }

        // GET: Matriculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matricula = _context.Matricula.Include(p => p.MatriculaAsignaturas).ThenInclude<Matricula, Matricula_Asignatura, Asignatura>(p => p.Asignatura).Include(p => p.Estudiante).Where(p => p.MatriculaId == id).ToList();

            if (matricula.Count == 0)
            {
                return NotFound();
            }

            return View(matricula.First());
        }

        // GET: Matriculas/Create
        public IActionResult Create(SelectedAsignaturasForMatriculaViewModel selectedAsignaturas)
        {
            Asignatura asignatura;
            int id;

            MatriculaCreateViewModel matricula = new MatriculaCreateViewModel();
            matricula.Matricula_Asignaturas = new List<Matricula_Asignatura>();
            if (selectedAsignaturas.IdsToAdd == null)
                ModelState.AddModelError("No ha seleccionado asignatura", $"Por favor, seleccione al menos una asignatura");
            else
                foreach (string ids in selectedAsignaturas.IdsToAdd)
                {
                    id = int.Parse(ids);
                    asignatura = _context.Asignatura.Include(m => m.Intensificacion).FirstOrDefault<Asignatura>(m => m.AsignaturaID.Equals(id));
                    matricula.Matricula_Asignaturas.Add(new Matricula_Asignatura() { cantidad = 1, Asignatura = asignatura });


                }
            Estudiante Estudiante = _context.Users.OfType<Estudiante>().FirstOrDefault<Estudiante>(u => u.UserName.Equals(User.Identity.Name));
            matricula.Nombre = Estudiante.Nombre;
            matricula.PrimerApellido = Estudiante.PrimerApellido;
            matricula.SegundoApellido = Estudiante.SegundoApellido;

            return View(matricula);
        }

        // POST: Matriculas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(MatriculaCreateViewModel matriculaViewModel, IList<Matricula_Asignatura> Matricula_Asignaturas, CreditCard CreditCard, PayPal PayPal)
        {
            Asignatura asignatura;
            Estudiante estudiante;
            Matricula matricula = new Matricula();
            matricula.PrecioTotal = 0;
            matricula.MatriculaAsignaturas = new List<Matricula_Asignatura>();
            ModelState.Clear();
            foreach (Matricula_Asignatura item in Matricula_Asignaturas)
            {
                asignatura = await _context.Asignatura.FirstOrDefaultAsync<Asignatura>(m => m.AsignaturaID == item.Asignatura.AsignaturaID);
                if (asignatura.cantidadAlumnos < item.cantidad)
                {
                    ModelState.AddModelError("", $"No hay suficiente material titulado {asignatura.NombreAsignatura}, por favor selecciones menos o igual que {asignatura.cantidadAlumnos}");
                    matriculaViewModel.Matricula_Asignaturas = Matricula_Asignaturas;
                }
                else
                {
                    if (item.cantidad > 0)
                    {
                        asignatura.cantidadAlumnos = asignatura.cantidadAlumnos - item.cantidad;
                        item.Asignatura = asignatura;
                        item.Matricula = matricula;
                        matricula.PrecioTotal += item.cantidad * asignatura.Precio;
                        matricula.MatriculaAsignaturas.Add(item);
                    }
                }
            }
            estudiante = await _context.Users.OfType<Estudiante>().FirstOrDefaultAsync<Estudiante>(u => u.UserName.Equals(User.Identity.Name));

            if (ModelState.ErrorCount > 0)
            {
                matriculaViewModel.Nombre = estudiante.Nombre;
                matriculaViewModel.PrimerApellido = estudiante.PrimerApellido;
                matriculaViewModel.SegundoApellido = estudiante.SegundoApellido;
                return View(matriculaViewModel);
            }

            if (matricula.PrecioTotal == 0)
            {
                matriculaViewModel.Nombre = estudiante.Nombre;
                matriculaViewModel.PrimerApellido = estudiante.PrimerApellido;
                matriculaViewModel.SegundoApellido = estudiante.SegundoApellido;
                ModelState.AddModelError("", $"Por favor seleccione al menos un material a comprar o cancele compra");
                matriculaViewModel.Matricula_Asignaturas = Matricula_Asignaturas;
                return View(matriculaViewModel);
            }

            matricula.Estudiante = estudiante;
            matricula.FechaMatricula = DateTime.Now;
            if (matriculaViewModel.MetodoPago == "PayPal")
                matricula.MetodoPago = matriculaViewModel.PayPal;
            else
                matricula.MetodoPago = matriculaViewModel.CreditCard;
            matricula.Residencia = matriculaViewModel.Direccion;
            _context.Add(matricula);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = matricula.MatriculaId });
        }

        // GET: Matriculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var matricula = await _context.Matricula.FindAsync(id);
            var matricula = await _context.Matricula.Include(p => p.MatriculaAsignaturas).ThenInclude<Matricula, Matricula_Asignatura, Asignatura>(i => i.Asignatura).Include(p => p.Estudiante).SingleOrDefaultAsync(m => m.MatriculaId == id);
            if (matricula == null)
            {
                return NotFound();
            }
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Id", matricula.EstudianteId);
            return View(matricula);
        }

        // POST: Matriculas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MatriculaId,EstudianteId,PrecioTotal,FechaMatricula,Residencia")] Matricula matricula)
        {
            if (id != matricula.MatriculaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matricula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatriculaExists(matricula.MatriculaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstudianteId"] = new SelectList(_context.Estudiante, "Id", "Id", matricula.EstudianteId);
            return View(matricula);
        }

        // GET: Matriculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matricula.SingleOrDefaultAsync(m => m.MatriculaId == id);

            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // POST: Matriculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var matricula = await _context.Matricula.FindAsync(id);
            _context.Matricula.Remove(matricula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatriculaExists(int id)
        {
            return _context.Matricula.Any(e => e.MatriculaId == id);
        }

        // GET: Matriculas/SelectAsignaturasForMatricula
        public IActionResult SelectAsignaturasForMatricula(string asignaturaNombre, string asignaturaIntensificacionSelected)
        {
            SelectAsignaturasForMatriculaViewModel selectAsignaturas = new SelectAsignaturasForMatriculaViewModel();
            selectAsignaturas.Intensificaciones = new SelectList(_context.Intensificacion.Select(g => g.NombreIntensificacion).ToList());
            selectAsignaturas.Asignaturas = _context.Asignatura.Include(m => m.Intensificacion).Where(m => m.cantidadAlumnos > 0);
            if (asignaturaNombre != null)
                selectAsignaturas.Asignaturas = selectAsignaturas.Asignaturas.Where(m => m.NombreAsignatura.Contains(asignaturaNombre));

            if (asignaturaIntensificacionSelected != null)
                selectAsignaturas.Asignaturas = selectAsignaturas.Asignaturas.Where(m => m.Intensificacion.NombreIntensificacion.Contains(asignaturaIntensificacionSelected));

            selectAsignaturas.Asignaturas.ToList();

            return View(selectAsignaturas);
        }

        // POST: Matriculas/SelectAsignaturasForMatricula
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectAsignaturasForMatricula(SelectedAsignaturasForMatriculaViewModel selectedAsignaturas)
        {
            if (selectedAsignaturas.IdsToAdd != null)
            {

                return RedirectToAction("Create", selectedAsignaturas);
            }
            //Saltará un mensaje de error en caso de que el estudiante no haya seleccionado ninguna asignatura
            ModelState.AddModelError(string.Empty, "Debe seleccionar al menos una asignatura");
            SelectAsignaturasForMatriculaViewModel selectAsignaturas = new SelectAsignaturasForMatriculaViewModel();
            selectAsignaturas.Intensificaciones = new SelectList(_context.Intensificacion.Select(g => g.NombreIntensificacion).ToList());
            selectAsignaturas.Asignaturas = _context.Asignatura.Include(m => m.Intensificacion).Where(m => m.cantidadAlumnos > 0).ToList();

            return View(selectAsignaturas);
        }
    }
}

