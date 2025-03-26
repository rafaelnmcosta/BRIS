import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';
import logo from '../../assets/BRIS-logo-horizontal.png';

const Header = () => {
    const { isAuthenticated, isLogged, logout } = useAuth();

    return (
        <header className="fixed top-0 left-0 w-full flex justify-between items-center px-8 py-2 border-b border-gray-300 bg-white shadow-md z-50">
            <div className="flex items-center">
                <img src={logo} alt="Logo" className="h-12" />
            </div>
            <nav>
                <ul className="flex space-x-6">
                    {isAuthenticated && isLogged ? (
                        <>
                            <li>
                                <Link to="/home" className="text-green-dark font-bold hover:text-green-light">Início</Link>
                            </li>
                            <li className="text-green-dark">|</li>
                            <li>
                                <Link to="/perfil" className="text-green-dark font-bold hover:text-green-light">Perfil</Link>
                            </li>
                            <li className="text-green-dark">|</li>
                            <li>
                                <Link to="/vinculos" className="text-green-dark font-bold hover:text-green-light">Trocar vínculo</Link>
                            </li>
                            <li className="text-green-dark">|</li>
                            <li>
                                <button onClick={logout} className="text-green-dark font-bold hover:text-green-light">
                                    Sair
                                </button>
                            </li>
                        </>
                    ) : isLogged ? (
                        <>
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
