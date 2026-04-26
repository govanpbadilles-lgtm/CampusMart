const EO = `<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"/><path stroke-linecap="round" stroke-linejoin="round" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.477 0 8.268 2.943 9.542 7-1.274 4.057-5.065 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"/></svg>`;
const EC = `<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M3 3l18 18M10.477 10.477A3 3 0 0113.5 13.5M6.357 6.357A9.965 9.965 0 002.458 12c1.274 4.057 5.065 7 9.542 7a9.966 9.966 0 005.643-1.745M9.878 9.878A3 3 0 0114.12 14.12M21.542 12c-1.274-4.057-5.065-7-9.542-7a9.96 9.96 0 00-3.17.516"/></svg>`;

let mode = 'login';

const PATH_RIGHT = "M 70,0 C 70,0 400,0 400,0 L 400,580 C 400,580 70,580 70,580 C 70,580 -30,435 60,290 C 140,145 -30,83 70,0 Z";
const PATH_LEFT = "M 0,0 C 0,0 330,0 330,0 C 330,0 430,83 350,290 C 270,497 430,580 330,580 L 0,580 Z";

const ovPath = document.getElementById('ovPath');

function go(m) {
    if (m === mode) return;
    mode = m;
    const card = document.getElementById('card');
    if (m === 'reg') {
        card.classList.add('reg');
        setTimeout(() => {
            if (ovPath) {
                ovPath.style.transition = `d 900ms cubic-bezier(0.8,0,0.2,1)`;
                ovPath.setAttribute('d', PATH_LEFT);
            }
        }, 50);
        const pReg = document.getElementById('pReg');
        if (pReg) pReg.scrollTop = 0;
    } else {
        card.classList.remove('reg');
        setTimeout(() => {
            if (ovPath) {
                ovPath.style.transition = `d 900ms cubic-bezier(0.8,0,0.2,1)`;
                ovPath.setAttribute('d', PATH_RIGHT);
            }
        }, 50);
    }
}

function togglePw(id, btn) {
    const el = document.getElementById(id);
    const show = el.type === 'password';
    el.type = show ? 'text' : 'password';
    btn.innerHTML = show ? EC : EO;
}

function toast(msg, bg = 'rgba(34, 197, 94, 0.9)') {
    const t = document.getElementById('toast');
    if (t) {
        t.innerHTML = `✅ ${msg}`;
        t.style.background = bg;
        t.classList.add('show');
        setTimeout(() => t.classList.remove('show'), 3500);
    }
}

function forgotPw(e) {
    e.preventDefault();
    toast('Password reset link sent to your email.', 'rgba(59, 130, 246, 0.9)');
}

function validateRegForm(e) {
    const pw = document.getElementById('Register_Password');
    const pwc = document.getElementById('Register_ConfirmPassword');
    const err = document.getElementById('pwerr');

    if (pw && pwc) {
        if (pw.value !== pwc.value) {
            if (err) err.classList.add('on');
            pwc.focus();
            return false;
        }
        if (err) err.classList.remove('on');
    }
    return true;
}

window.addEventListener('load', () => {
    document.querySelectorAll('input[type="text"], input[type="password"], input[type="email"]').forEach(el => {
        el.value = '';
    });
});
