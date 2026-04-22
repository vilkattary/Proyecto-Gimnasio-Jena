function showSection(id, btn) {
    document.querySelectorAll('.section').forEach(s => s.classList.remove('visible'));
    document.querySelectorAll('.nav-pill').forEach(b => b.classList.remove('active'));
    document.getElementById('sec-' + id).classList.add('visible');
    btn.classList.add('active');
}

const clases = [
    { name: 'HIIT Explosivo', val: 420, max: 420 },
    { name: 'Yoga Restore', val: 380, max: 420 },
    { name: 'Spinning Pro', val: 350, max: 420 },
    { name: 'Pilates Core', val: 300, max: 420 },
    { name: 'Box & Burn', val: 265, max: 420 },
    { name: 'Zumba Fit', val: 240, max: 420 },
    { name: 'Funcional 360', val: 210, max: 420 },
];
const rankEl = document.getElementById('rankingClases');
clases.forEach((c, i) => {
    const pct = Math.round(c.val / c.max * 100);
    rankEl.innerHTML += `<div class="rank-row">
    <span class="rank-num ${i === 0 ? 'gold' : ''}">${i + 1}</span>
    <span class="rank-name">${c.name}</span>
    <div class="rank-bar-wrap"><div class="rank-bar" style="width:${pct}%"></div></div>
    <span class="rank-val">${c.val}</span>
  </div>`;
});

const trainers = [
    { init: 'VG', name: 'Valeria Gutiérrez', classes: 'Spinning, HIIT', n: 24, cls: 'a', rating: '4.8' },
    { init: 'MR', name: 'Marcos Rojas', classes: 'Yoga, Pilates', n: 22, cls: 'b', rating: '4.9' },
    { init: 'SQ', name: 'Sofía Quirós', classes: 'Funcional, Box', n: 18, cls: 'c', rating: '4.7' },
    { init: 'AF', name: 'Andrés Flores', classes: 'HIIT, Funcional', n: 16, cls: 'd', rating: '4.6' },
    { init: 'DM', name: 'Daniela Mora', classes: 'Zumba, Pilates', n: 14, cls: 'e', rating: '4.5' },
];
const tcEl = document.getElementById('trainerClases');
trainers.forEach((t, i) => {
    const pct = Math.round(t.n / 24 * 100);
    tcEl.innerHTML += `<div class="rank-row">
    <span class="rank-num">${i + 1}</span>
    <span class="rank-name">${t.name.split(' ')[0]} ${t.name.split(' ')[1].charAt(0)}.</span>
    <div class="rank-bar-wrap"><div class="rank-bar" style="width:${pct}%;background:#1D9E75"></div></div>
    <span class="rank-val">${t.n}</span>
  </div>`;
});
const tlEl = document.getElementById('trainerList');
trainers.forEach(t => {
    tlEl.innerHTML += `<div class="trainer-row">
    <div class="avatar ${t.cls}">${t.init}</div>
    <div class="trainer-info">
      <div class="trainer-name">${t.name}</div>
      <div class="trainer-classes">${t.classes}</div>
    </div>
    <span class="badge badge-blue">★ ${t.rating}</span>
  </div>`;
});

const surveys = [
    { q: 'Instalaciones y equipos', score: '4.7', dist: [2, 4, 8, 38, 48] },
    { q: 'Calidad de las clases', score: '4.8', dist: [1, 2, 5, 35, 57] },
    { q: 'Atención del personal', score: '4.5', dist: [2, 5, 10, 42, 41] },
    { q: 'Relación precio / valor', score: '4.2', dist: [3, 8, 15, 45, 29] },
];
const sgEl = document.getElementById('surveyGrid');
const barColors = ['#E24B4A', '#EF9F27', '#BA7517', '#1D9E75', '#185FA5'];
surveys.forEach(s => {
    const stars = '★'.repeat(Math.round(parseFloat(s.score))) + '☆'.repeat(5 - Math.round(parseFloat(s.score)));
    let bars = '';
    s.dist.forEach((pct, i) => {
        bars += `<div class="sbar-row"><span>${i + 1}★</span><div class="sbar-wrap"><div class="sbar" style="width:${pct}%;background:${barColors[i]}"></div></div><span>${pct}%</span></div>`;
    });
    sgEl.innerHTML += `<div class="survey-item">
    <div class="survey-q">${s.q}</div>
    <div class="survey-score">${s.score}</div>
    <div class="star-row" style="color:#BA7517;font-size:14px">${stars}</div>
    <div class="survey-bars">${bars}</div>
  </div>`;
});

new Chart(document.getElementById('chartNuevos'), {
    type: 'bar',
    data: { labels: ['Ene', 'Feb', 'Mar', 'Abr'], datasets: [{ label: 'Nuevos', data: [28, 32, 33, 41], backgroundColor: '#185FA5', borderRadius: 4 }] },
    options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { display: false } }, scales: { x: { grid: { display: false } }, y: { beginAtZero: true, grid: { color: 'rgba(0,0,0,.05)' }, ticks: { stepSize: 10 } } } }
});

new Chart(document.getElementById('chartMemb'), {
    type: 'doughnut',
    data: { labels: ['Mensual', 'Trimestral', 'Anual'], datasets: [{ data: [52, 28, 20], backgroundColor: ['#185FA5', '#1D9E75', '#BA7517'], borderWidth: 0 }] },
    options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { display: false } }, cutout: '65%' }
});

new Chart(document.getElementById('chartCats'), {
    type: 'bar',
    data: { labels: ['Funcional', 'Yoga', 'Spinning', 'Pilates', 'Boxeo', 'Zumba'], datasets: [{ label: 'Asistentes', data: [520, 410, 390, 320, 280, 270], backgroundColor: ['#185FA5', '#1D9E75', '#BA7517', '#D4537E', '#7F77DD', '#D85A30'], borderRadius: 4 }] },
    options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { display: false } }, scales: { x: { grid: { display: false } }, y: { beginAtZero: true, grid: { color: 'rgba(0,0,0,.05)' } } } }
});

new Chart(document.getElementById('chartTrainers'), {
    type: 'bar',
    data: { labels: ['Valeria', 'Marcos', 'Sofía', 'Andrés', 'Daniela', 'Luis', 'Carmen', 'Roberto'], datasets: [{ label: 'Clases', data: [24, 22, 18, 16, 14, 12, 10, 8], backgroundColor: '#1D9E75', borderRadius: 4 }] },
    options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { display: false } }, scales: { x: { grid: { display: false } }, y: { beginAtZero: true, ticks: { stepSize: 5 }, grid: { color: 'rgba(0,0,0,.05)' } } } }
});