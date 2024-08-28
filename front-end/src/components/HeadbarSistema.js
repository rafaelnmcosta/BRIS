import React from 'react';
import { Link } from 'react-router-dom';
import './HeadbarSistema.css';

import logo from '../assets/BRIS-logo-horizontal.png'

function HeadbarSistema() {
    const id = localStorage.getItem("userId");
    const handleLogout = async () => {
        localStorage.removeItem('jwtToken');
    }
    
    return (
        <nav className='navbar'>
            <div className='container'>
            <div className='navbar-item'>
                <img src={logo} alt="Logo" style={{ width: '200px', height: 'auto' }} />
            </div>
            <div className='navbar-menu'>
                <Link to="/">Página inicial</Link>
                <p>|</p>
                <Link to = {`/perfil/${id}`} >Perfil</Link>
                <p>|</p>
                <Link onClick={handleLogout} to="/login">Sair</Link>
            </div>
            </div>
        </nav>
    );
}

export default HeadbarSistema;