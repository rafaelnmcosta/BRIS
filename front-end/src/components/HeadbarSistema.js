import React from 'react';
import { Link } from 'react-router-dom';
import './HeadbarSistema.css';

import logo from '../assets/BRIS-logo-horizontal.png'

function HeadbarSistema() {
    return (
        <nav className='navbar'>
            <div className='container'>
            <div className='navbar-item'>
                <img src={logo} alt="Logo" style={{ width: '200px', height: 'auto' }} />
            </div>
            <div className='navbar-menu'>
                <Link to="/configuracoes">Configurações</Link>
                <p>|</p>
                <Link to="/login">Sair</Link>
            </div>
            </div>
        </nav>
    );
}

export default HeadbarSistema;