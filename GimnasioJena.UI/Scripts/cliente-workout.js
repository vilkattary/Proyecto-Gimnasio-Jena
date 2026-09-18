// Módulo Cliente: logging de entrenamiento (Etapa 1).
// Carga la rutina de la clase en un modal Bootstrap 5 y envía los resultados vía AJAX.
(function () {
    "use strict";

    window.ClienteWorkout = window.ClienteWorkout || {
        partialUrl: "",
        saveUrl: ""
    };

    function obtenerToken() {
        var input = document.querySelector('input[name="__RequestVerificationToken"]');
        return input ? input.value : "";
    }

    function obtenerModal() {
        var el = document.getElementById("workoutClientModal");
        if (!el || typeof bootstrap === "undefined") { return null; }
        return bootstrap.Modal.getOrCreateInstance(el);
    }

    function leerSeries() {
        var series = [];
        document.querySelectorAll("#workoutClientForm .wk-ejercicio-cliente").forEach(function (bloque) {
            var exerciseId = parseInt(bloque.getAttribute("data-exercise-id"), 10);
            var tipoMetrica = parseInt(bloque.getAttribute("data-tipo-metrica"), 10);

            bloque.querySelectorAll(".wk-series-body tr").forEach(function (fila) {
                var hecho = fila.querySelector(".wk-hecho");
                if (!hecho || !hecho.checked) { return; }

                var setNumber = parseInt(fila.getAttribute("data-set"), 10);
                var repsVal = fila.querySelector(".wk-reps").value;
                var pesoVal = fila.querySelector(".wk-peso").value;

                series.push({
                    WorkoutExerciseId: exerciseId,
                    SetNumber: setNumber,
                    RepsCompleted: repsVal !== "" ? parseInt(repsVal, 10) : null,
                    WeightUsedKg: pesoVal !== "" ? parseFloat(pesoVal) : null,
                    DurationSeconds: tipoMetrica === 2 && repsVal !== "" ? parseInt(repsVal, 10) : null,
                    DistanceMeters: null
                });
            });
        });
        return series;
    }

    function mostrarMensaje(html) {
        var cont = document.getElementById("wkMensajeCliente");
        if (cont) { cont.innerHTML = html; }
    }

    function enlazarGuardado() {
        var boton = document.getElementById("wkGuardarEntrenamiento");
        if (!boton) { return; }

        boton.addEventListener("click", function () {
            var form = document.getElementById("workoutClientForm");
            if (!form) { return; }

            var series = leerSeries();
            if (!series.length) {
                mostrarMensaje('<div class="alert alert-warning mb-2">Marca al menos una serie como completada.</div>');
                return;
            }

            var payload = {
                ClassInstanceId: parseInt(form.getAttribute("data-class-id"), 10),
                WorkoutDayTemplateId: parseInt(form.getAttribute("data-template-id"), 10),
                Notes: (document.getElementById("wkNotasCliente") || {}).value || "",
                Series: series
            };

            boton.disabled = true;
            mostrarMensaje('<div class="text-muted small mb-2">Guardando…</div>');

            fetch(window.ClienteWorkout.saveUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    "RequestVerificationToken": obtenerToken()
                },
                body: JSON.stringify(payload)
            })
                .then(function (r) { return r.json(); })
                .then(function (resp) {
                    if (resp && resp.success) {
                        var extra = resp.newRecord
                            ? ' <strong>🏆 ¡Nuevo récord personal!</strong>'
                            : "";
                        mostrarMensaje('<div class="alert alert-success mb-0">' + resp.message + extra + '</div>');
                        setTimeout(function () {
                            var modal = obtenerModal();
                            if (modal) { modal.hide(); }
                        }, 1500);
                    } else {
                        mostrarMensaje('<div class="alert alert-danger mb-0">' +
                            ((resp && resp.message) || "No se pudo guardar el entrenamiento.") + '</div>');
                    }
                })
                .catch(function () {
                    mostrarMensaje('<div class="alert alert-danger mb-0">Ocurrió un error al guardar.</div>');
                })
                .finally(function () {
                    boton.disabled = false;
                });
        });
    }

    // Punto de entrada llamado desde el botón de cada tarjeta de clase.
    window.loadClassWorkout = function (classId) {
        var modal = obtenerModal();
        var body = document.getElementById("workoutClientModalBody");
        if (!modal || !body) { return; }

        body.innerHTML = '<p class="text-muted">Cargando…</p>';
        modal.show();

        fetch(window.ClienteWorkout.partialUrl + "?classId=" + encodeURIComponent(classId), {
            headers: { "X-Requested-With": "XMLHttpRequest" }
        })
            .then(function (r) {
                if (!r.ok) { throw new Error("No autorizado o error del servidor."); }
                return r.text();
            })
            .then(function (html) {
                body.innerHTML = html;
                enlazarGuardado();
            })
            .catch(function () {
                body.innerHTML = '<div class="alert alert-danger mb-0">No se pudo cargar el entrenamiento.</div>';
            });
    };
})();
