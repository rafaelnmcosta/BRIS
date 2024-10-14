import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';
import logo from '../../assets/BRIS-logo-horizontal.png';

const Header = () => {
  const { isAuthenticated, logout } = useAuth();

  return (
    <header className="flex justify-between items-center px-8 py-2 border-b border-gray-300">
        <div className="flex items-center">
            <img src={logo} alt="Logo" className="h-12  " />
        </div>
        <nav className="">
            <ul className="flex space-x-6">
                {isAuthenticated ? (
                <>
                    <li>
                    <Link to="/home" className="text-green-dark font-bold hover:text-green-light">Início</Link>
                    </li>
                    <li className="text-green-dark">|</li>
                    <li>
                    <Link to="/ajuda" className="text-green-dark font-bold hover:text-green-light">Ajuda</Link>
                    </li>
                    <li className="text-green-dark">|</li>
                    <li>
                    <Link to="/lierfil" className="text-green-dark font-bold hover:text-green-light">Perfil</Link>
                    </li>
                    <li className="text-green-dark">|</li>
                    <li>
                    <button onClick={logout} className="text-green-dark font-bold hover:text-green-light">
                        Sair
                    </button>
                    </li>
                </>
                ) : (
                <>
                    <li>
                    <Link to="/sobre" className="text-green-dark font-bold hover:text-green-light">Sobre</Link>
                    </li>
                    <li className="text-green-dark">|</li>
                    <li>
                    <Link to="/cadastro" className="text-green-dark font-bold hover:text-green-light">Cadastro</Link>
                    </li>
                    <li className="text-green-dark">|</li>
                    <li>
                    <Link to="/login" className="text-green-dark font-bold hover:text-green-light">Login</Link>
                    </li>
                </>
                )}
            </ul>
        </nav>
    </header>
  );
};

export default Header;
