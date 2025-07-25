import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { formatarCPF, formatarTelefone } from '../../services/Formatter'

const TemplatePerfil = ({ dadosPerfil }) => {
    const { userData } = useAuth();
    const navigate = useNavigate();
    return (
        <div className="container mx-auto pt-8 h-fit">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-green-dark">Perfil</h1>

                <div className="flex gap-4 w-fit">
                    <BotaoPrimario
                        texto="Editar"
                        onClick={() => navigate('/perfil/editar')}
                    />
                </div>
            </div>

            {/* Informações do usuário */}
            <div className="flex flex-col lg:flex-row gap-2 text-green-dark">
                {/* Dados principais */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-y-6 gap-x-32 bg-white px-8 py-10 rounded-lg shadow w-full">
                    <div>
                        <p className="text-sm text-gray-500">Nome</p>
                        <p className="text-base font-medium">{dadosPerfil.nome}</p>
                    </div>
                    <div>
                        <p className="text-sm text-gray-500">Email</p>
                        <p className="text-base font-medium">{dadosPerfil.email}</p>
                    </div>
                    <div>
                        <p className="text-sm text-gray-500">CPF</p>
                        <p className="text-base font-medium">{formatarCPF(dadosPerfil.cpf)}</p>
                    </div>
                    <div>
                        <p className="text-sm text-gray-500">Telefone</p>
                        <p className="text-base font-medium">{formatarTelefone(dadosPerfil.telefone)}</p>
                    </div>
                </div>

                {/* Dados complementares */}
                <div className="bg-white px-6 py-6 rounded-lg shadow w-full lg:w-1/2">
                    <div className="grid grid-cols-1 gap-4">
                        <div>
                            <p className="text-sm text-gray-500">Perfil</p>
                            <p className="text-base font-medium capitalize">{dadosPerfil.role}</p>
                        </div>

                        {userData.agroindustria !== "N/A" && userData.agroindustria !== 'N/A' && (
                            <div>
                                <p className="text-sm text-gray-500">Agroindústria</p>
                                <p className="text-base font-medium">{userData.agroindustria}</p>
                            </div>
                        )}

                        {userData.granja !== "N/A" && (
                            <div>
                                <p className="text-sm text-gray-500">Granja</p>
                                <p className="text-base font-medium">{userData.granja}</p>
                            </div>
                        )}
                    </div>
                </div>
            </div>

        </div>
    );
};

export default TemplatePerfil;
