// Elementos DOM
const sidebar = document.querySelector('.sidebar');
const mainContent = document.querySelector('.main-content');
const sidebarToggle = document.getElementById('sidebarToggle');
const sidebarOverlay = document.getElementById('sidebarOverlay');
const themeToggle = document.getElementById('themeToggle');

// Função para verificar se é mobile
function checkIsMobile() {
    return window.innerWidth <= 768;
}

// Funções para localStorage
function saveSidebarState(isCollapsed) {
    localStorage.setItem('sidebarCollapsed', isCollapsed);
}

function getSidebarState() {
    return localStorage.getItem('sidebarCollapsed') === 'true';
}

function saveThemeState(theme) {
    localStorage.setItem('theme', theme);
}

function getThemeState() {
    return localStorage.getItem('theme') || 'light';
}

// Aplicar tema do Bootstrap
function applyTheme(theme) {
    document.documentElement.setAttribute('data-bs-theme', theme);
    saveThemeState(theme);
    
    // Atualizar ícone
    if (themeToggle) {
        const icon = themeToggle.querySelector('i');
        if (icon) {
            icon.className = theme === 'dark' ? 'bi bi-sun' : 'bi bi-moon';
        }
    }
}

// Aplicar estado salvo ao carregar a página
function applySavedStates() {
    // Aplicar tema
    const savedTheme = getThemeState();
    applyTheme(savedTheme);

    // Aplicar estado da sidebar (apenas no desktop)
    if (!checkIsMobile()) {
        const isSidebarCollapsed = getSidebarState();
        if (isSidebarCollapsed && sidebar) {
            sidebar.classList.add('collapsed');
            if (mainContent) {
                mainContent.style.marginLeft = '70px';
            }
            // Atualizar ícone do botão
            if (sidebarToggle) {
                const icon = sidebarToggle.querySelector('i');
                if (icon) {
                    icon.classList.remove('bi-list');
                    icon.classList.add('bi-arrow-right');
                }
            }
        }
    }
}

// Alternar sidebar
if (sidebarToggle) {
    sidebarToggle.addEventListener('click', function () {
        const isMobile = checkIsMobile();
        
        if (isMobile) {
            // No mobile, usa a classe 'show' para exibir/ocultar
            sidebar.classList.toggle('show');
            if (sidebarOverlay) sidebarOverlay.classList.toggle('show');
        } else {
            // No desktop, usa a classe 'collapsed' para expandir/recolher
            sidebar.classList.toggle('collapsed');

            // Salvar estado no localStorage
            const isCollapsed = sidebar.classList.contains('collapsed');
            saveSidebarState(isCollapsed);

            // Atualizar margem do conteúdo principal
            if (mainContent) {
                if (isCollapsed) {
                    mainContent.style.marginLeft = '70px';
                } else {
                    mainContent.style.marginLeft = '250px';
                }
            }

            // Alternar ícone do botão
            const icon = this.querySelector('i');
            if (icon) {
                if (isCollapsed) {
                    icon.classList.remove('bi-list');
                    icon.classList.add('bi-arrow-right');
                } else {
                    icon.classList.remove('bi-arrow-right');
                    icon.classList.add('bi-list');
                }
            }
        }
    });
}

// Fechar sidebar ao clicar no overlay (mobile)
if (sidebarOverlay) {
    sidebarOverlay.addEventListener('click', function () {
        sidebar.classList.remove('show');
        this.classList.remove('show');
    });
}

// Fechar sidebar ao clicar em um link (mobile)
const sidebarLinks = document.querySelectorAll('.sidebar .nav-link');
sidebarLinks.forEach(link => {
    link.addEventListener('click', function () {
        if (checkIsMobile()) {
            sidebar.classList.remove('show');
            if (sidebarOverlay) sidebarOverlay.classList.remove('show');
        }
    });
});

// Alternar tema usando sistema nativo do Bootstrap
if (themeToggle) {
    themeToggle.addEventListener('click', function () {
        const currentTheme = getThemeState();
        const newTheme = currentTheme === 'light' ? 'dark' : 'light';
        applyTheme(newTheme);
    });
}

// Responsividade - atualizar comportamento ao redimensionar
function handleResize() {
    const isMobile = checkIsMobile();

    if (isMobile) {
        // Modo mobile - sidebar escondida por padrão
        if (sidebar) sidebar.classList.remove('collapsed');
        if (sidebar) sidebar.classList.remove('show');
        if (sidebarOverlay) sidebarOverlay.classList.remove('show');
        if (mainContent) mainContent.style.marginLeft = '0';

        // Atualizar ícone do botão
        if (sidebarToggle) {
            const icon = sidebarToggle.querySelector('i');
            if (icon) {
                icon.classList.remove('bi-arrow-right');
                icon.classList.add('bi-list');
            }
        }
    } else {
        // Modo desktop - restaurar estado salvo
        if (sidebar) sidebar.classList.remove('show');
        if (sidebarOverlay) sidebarOverlay.classList.remove('show');

        // Aplicar estado salvo da sidebar
        const isSidebarCollapsed = getSidebarState();
        if (sidebar) {
            if (isSidebarCollapsed) {
                sidebar.classList.add('collapsed');
                if (mainContent) mainContent.style.marginLeft = '70px';
                
                // Atualizar ícone do botão
                if (sidebarToggle) {
                    const icon = sidebarToggle.querySelector('i');
                    if (icon) {
                        icon.classList.remove('bi-list');
                        icon.classList.add('bi-arrow-right');
                    }
                }
            } else {
                sidebar.classList.remove('collapsed');
                if (mainContent) mainContent.style.marginLeft = '250px';
                
                // Atualizar ícone do botão
                if (sidebarToggle) {
                    const icon = sidebarToggle.querySelector('i');
                    if (icon) {
                        icon.classList.remove('bi-arrow-right');
                        icon.classList.add('bi-list');
                    }
                }
            }
        }
    }
}

// Executar na carga inicial e no redimensionamento
window.addEventListener('load', function() {
    applySavedStates();
    handleResize();
});
window.addEventListener('resize', handleResize);

