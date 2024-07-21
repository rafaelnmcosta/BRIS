import React from 'react';
import { Link } from 'react-router-dom';
import './HeadbarLogin.css';

import logo from '../assets/BRIS-logo-horizontal.png'

function HeadbarLogin() {
    return (
        <nav className='navbar'>
            <div className='container'>
                <div className='navbar-item'>
                    <img src={logo} alt="Logo" style={{ width: '200px', height: 'auto' }} />
                </div>
                <div className='navbar-menu'>
                    <Link to="/sobre">Sobre</Link>
                    <p>|</p>
                    <Link to="/cadastro">Cadastro</Link>
                    <p>|</p>
                    <Link to="/login">Login</Link>
                </div>
            </div>
        </nav>
    );
}

export default HeadbarLogin;