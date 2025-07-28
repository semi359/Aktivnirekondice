document.addEventListener('DOMContentLoaded', function () {
    const toggle = document.getElementById('themeToggle'); if (!toggle) return; const darkClass = 'dark-mode'; const body = document.body; if (localStorage.getItem('darkMode') === 'true') { body.classList.add(darkClass) }
    toggle.addEventListener('click', () => { const isDark = body.classList.toggle(darkClass); localStorage.setItem('darkMode', isDark) })
})