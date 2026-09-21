// Editor de rutina por día (mesociclo de 4 semanas) — Fase 4
// Se apoya en los endpoints del MesocicloController y trabaja siempre con DTOs.
(function () {
    "use strict";

    var SEMANAS = [1, 2, 3, 4];

    function nombreMetrica(tipo) {
        switch (parseInt(tipo, 10)) {
            case 2: return "Tiempo";
            case 3: return "Distancia";
            default: return "Repeticiones";
        }
    }

    function crearProgresionVacia(numeroSemana) {
        return {
            idProgresion: 0,
            NumeroSemana: numeroSemana,
            Series: null,
            RepeticionesObjetivo: "",
            DuracionSegundos: null,
            CargaOrpeSugerido: ""
        };
    }

    function normalizarProgresiones(progresiones) {
        var mapa = {};
        (progresiones || []).forEach(function (p) {
            mapa[p.NumeroSemana] = p;
        });
        return SEMANAS.map(function (semana) {
            return mapa[semana] || crearProgresionVacia(semana);
        });
    }

    function construirFilaEjercicio(ejercicio, indice) {
        var esTiempo = parseInt(ejercicio.TipoMetrica, 10) === 2;
        var progresiones = normalizarProgresiones(ejercicio.Progresiones);

        var filasSemanas = progresiones.map(function (p) {
            var campoMetrica = esTiempo
                ? '<input type="number" min="0" class="form-control form-control-sm wk-duracion" ' +
                  'aria-label="Duración semana ' + p.NumeroSemana + '" value="' + (p.DuracionSegundos || "") + '" />'
                : '<input type="text" class="form-control form-control-sm wk-repeticiones" ' +
                  'aria-label="Repeticiones semana ' + p.NumeroSemana + '" value="' + (p.RepeticionesObjetivo || "") + '" />';

            return '' +
                '<tr data-semana="' + p.NumeroSemana + '">' +
                '  <th scope="row">Semana ' + p.NumeroSemana + '</th>' +
                '  <td><input type="number" min="0" class="form-control form-control-sm wk-series" ' +
                '     aria-label="Series semana ' + p.NumeroSemana + '" value="' + (p.Series != null ? p.Series : "") + '" /></td>' +
                '  <td>' + campoMetrica + '</td>' +
                '  <td><input type="text" class="form-control form-control-sm wk-carga" ' +
                '     aria-label="Carga o RPE semana ' + p.NumeroSemana + '" value="' + (p.CargaOrpeSugerido || "") + '" /></td>' +
                '</tr>';
        }).join("");

        var encabezadoMetrica = esTiempo ? "Duración (seg)" : "Repeticiones";

        return '' +
            '<div class="border rounded p-3 mb-3 wk-ejercicio" data-indice="' + indice + '">' +
            '  <div class="row g-2 mb-2">' +
            '    <div class="col-md-6">' +
            '      <label class="info-label">Nombre del ejercicio</label>' +
            '      <input type="text" class="form-control form-control-sm mt-1 wk-nombre" value="' + (ejercicio.NombreEjercicio || "") + '" />' +
            '    </div>' +
            '    <div class="col-md-4">' +
            '      <label class="info-label">Métrica</label>' +
            '      <select class="form-control form-control-sm mt-1 wk-tipo-metrica">' +
            '        <option value="1"' + (!esTiempo && parseInt(ejercicio.TipoMetrica, 10) !== 3 ? " selected" : "") + '>Repeticiones</option>' +
            '        <option value="2"' + (esTiempo ? " selected" : "") + '>Tiempo</option>' +
            '        <option value="3"' + (parseInt(ejercicio.TipoMetrica, 10) === 3 ? " selected" : "") + '>Distancia</option>' +
            '      </select>' +
            '    </div>' +
            '    <div class="col-md-2 d-flex align-items-end justify-content-end">' +
            '      <button type="button" class="btn btn-outline-danger btn-sm wk-eliminar-ejercicio" aria-label="Eliminar ejercicio">' +
            '        <i class="bi bi-trash"></i></button>' +
            '    </div>' +
            '  </div>' +
            '  <div class="mb-2">' +
            '    <label class="info-label">Notas</label>' +
            '    <input type="text" class="form-control form-control-sm mt-1 wk-notas" value="' + (ejercicio.Notas || "") + '" />' +
            '  </div>' +
            '  <div class="table-responsive">' +
            '    <table class="table table-sm table-bordered align-middle mb-0">' +
            '      <thead><tr>' +
            '        <th scope="col">Semana</th><th scope="col">Series</th>' +
            '        <th scope="col" class="wk-encabezado-metrica">' + encabezadoMetrica + '</th>' +
            '        <th scope="col">Carga / RPE</th>' +
            '      </tr></thead>' +
            '      <tbody>' + filasSemanas + '</tbody>' +
            '    </table>' +
            '  </div>' +
            '</div>';
    }

    function leerEjercicioDesdeDom($ejercicio) {
        var tipoMetrica = parseInt($ejercicio.find(".wk-tipo-metrica").val(), 10);
        var esTiempo = tipoMetrica === 2;

        var progresiones = [];
        $ejercicio.find("tbody tr").each(function () {
            var $fila = window.jQuery(this);
            var semana = parseInt($fila.data("semana"), 10);
            var series = $fila.find(".wk-series").val();
            var carga = $fila.find(".wk-carga").val();

            progresiones.push({
                idProgresion: 0,
                NumeroSemana: semana,
                Series: series !== "" ? parseInt(series, 10) : null,
                RepeticionesObjetivo: esTiempo ? "" : ($fila.find(".wk-repeticiones").val() || ""),
                DuracionSegundos: esTiempo
                    ? ($fila.find(".wk-duracion").val() !== "" ? parseInt($fila.find(".wk-duracion").val(), 10) : null)
                    : null,
                CargaOrpeSugerido: carga || ""
            });
        });

        return {
            idEjercicio: 0,
            NombreEjercicio: $ejercicio.find(".wk-nombre").val() || "",
            TipoMetrica: tipoMetrica,
            OrdenIndice: parseInt($ejercicio.data("indice"), 10),
            Notas: $ejercicio.find(".wk-notas").val() || "",
            Progresiones: progresiones
        };
    }

    // Auto-cascada: si Semanas 2/3/4 están vacías, replican el valor de Semana 1.
    function aplicarAutoCascada($ejercicio) {
        ["wk-series", "wk-repeticiones", "wk-duracion", "wk-carga"].forEach(function (clase) {
            var $filas = $ejercicio.find("tbody tr");
            var $primera = $filas.eq(0).find("." + clase);
            if (!$primera.length) { return; }
            var valorBase = $primera.val();
            if (valorBase === "") { return; }
            $filas.each(function (idx) {
                if (idx === 0) { return; }
                var $campo = window.jQuery(this).find("." + clase);
                if ($campo.length && $campo.val() === "") {
                    $campo.val(valorBase);
                }
            });
        });
    }

    window.WorkoutEditor = {
        inicializar: function (rootSelector) {
            var $ = window.jQuery;
            var $root = $(rootSelector).find("#workoutEditorRoot");
            if (!$root.length) { return; }

            var $contenedor = $root.find("#wkEjerciciosContenedor");
            var indiceActual = 0;

            function agregarEjercicio(ejercicio) {
                var html = construirFilaEjercicio(ejercicio, indiceActual);
                indiceActual++;
                $contenedor.append(html);
            }

            // Cargar datos iniciales
            var datos = [];
            try {
                datos = JSON.parse($root.find("#wkDatosIniciales").text() || "[]") || [];
            } catch (e) { datos = []; }

            if (datos.length) {
                datos.forEach(function (ej) { agregarEjercicio(ej); });
            } else {
                agregarEjercicio({ TipoMetrica: 1, Progresiones: [] });
            }

            $root.on("click", "#wkAgregarEjercicio", function () {
                agregarEjercicio({ TipoMetrica: 1, Progresiones: [] });
            });

            $root.on("click", ".wk-eliminar-ejercicio", function () {
                var $todos = $contenedor.find(".wk-ejercicio");
                if ($todos.length <= 1) { return; }
                $(this).closest(".wk-ejercicio").remove();
            });

            // Cambio de métrica: reconstruye la fila conservando datos.
            $root.on("change", ".wk-tipo-metrica", function () {
                var $ejercicio = $(this).closest(".wk-ejercicio");
                var datosEjercicio = leerEjercicioDesdeDom($ejercicio);
                var indice = $ejercicio.data("indice");
                $ejercicio.replaceWith(construirFilaEjercicio(datosEjercicio, indice));
            });

            // Auto-cascada al salir del campo de Semana 1.
            $root.on("blur", "tbody tr:first-child input", function () {
                aplicarAutoCascada($(this).closest(".wk-ejercicio"));
            });

            $root.on("click", "#wkGuardarPlantilla", function () {
                var $boton = $(this);
                var $mensaje = $root.find("#wkMensaje");
                $mensaje.html("");

                var ejercicios = [];
                $contenedor.find(".wk-ejercicio").each(function (idx) {
                    var ej = leerEjercicioDesdeDom($(this));
                    ej.OrdenIndice = idx + 1;
                    ejercicios.push(ej);
                });

                var payload = {
                    idPlantillaDia: parseInt($root.data("id-plantilla"), 10) || 0,
                    idMesociclo: parseInt($root.data("id-mesociclo"), 10) || 0,
                    DiaSemana: parseInt($root.find("#wkDiaSemana").val(), 10),
                    AreaEnfoque: $root.find("#wkAreaEnfoque").val() || "",
                    OrdenIndice: 0,
                    Ejercicios: ejercicios
                };

                var token = $('input[name="__RequestVerificationToken"]').first().val();

                $boton.prop("disabled", true);

                $.ajax({
                    url: window.WorkoutEditor.saveUrl,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(payload),
                    headers: token ? { "RequestVerificationToken": token } : {},
                    success: function (resp) {
                        if (resp && resp.success) {
                            $mensaje.html('<div class="alert alert-success mb-0">' + resp.message + '</div>');
                            if (window.WorkoutEditor.onSaved) {
                                window.WorkoutEditor.onSaved(resp);
                            }
                        } else {
                            $mensaje.html('<div class="alert alert-danger mb-0">' +
                                ((resp && resp.message) || "No se pudo guardar la rutina.") + '</div>');
                        }
                    },
                    error: function () {
                        $mensaje.html('<div class="alert alert-danger mb-0">Ocurrió un error al guardar la rutina.</div>');
                    },
                    complete: function () {
                        $boton.prop("disabled", false);
                    }
                });
            });
        }
    };
})();
