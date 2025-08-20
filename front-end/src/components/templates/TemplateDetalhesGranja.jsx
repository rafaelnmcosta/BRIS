import React from 'react';
import { useNavigate } from 'react-router-dom';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { formatarTelefone, formatarData } from '../../services/Formatter';
import { granjas } from '../../api/granjasAPI';

const TemplateDetalhesGranja = ({ dados }) => {
    const navigate = useNavigate();
    console.log('Dados recebidos:', dados);
    return (
        <div className="container mx-auto pt-8 h-fit">
            {/* Header */}
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-green-dark">
                    Detalhes da Granja
                </h1>
                <div className="flex gap-4">
                    <BotaoPrimario
                        texto="Editar"
                        onClick={() => navigate(`/granjas/${dados.id}/editar`)}
                    />
                    <BotaoPrimario
                        texto={dados.ativo ? 'Inativar' : 'Ativar'}
                        onClick={async () => {
                            try {
                                if (dados.ativo) {
                                    await granjas.desativarGranja(dados.id);
                                } else {
                                    await granjas.ativarGranja(dados.id);
                                }
                                navigate(dados.ativo ? `/granjas/inativas` : `/granjas`);
                            } catch (error) {
                                console.error('Erro ao alterar status da granja:', error);
                            }
                        }}
                    />
                </div>
            </div>

            {/* Conteúdo da Granja */}
            <div className="bg-white shadow-lg rounded-lg p-8 grid grid-cols-1 md:grid-cols-2 gap-y-6 gap-x-16 text-green-dark mb-8">
                <div>
                    <p className="text-sm text-gray-500">Nome da Granja</p>
                    <p className="text-base font-medium">{dados.nomePropriedade}</p>
                </div>

                <div>
                    <p className="text-sm text-gray-500">Telefone</p>
                    <p className="text-base font-medium">{formatarTelefone(dados.telefone)}</p>
                </div>

                <div>
                    <p className="text-sm text-gray-500">E-mail</p>
                    <p className="text-base font-medium">{dados.email}</p>
                </div>

                <div>
                    <p className="text-sm text-gray-500">Data de Cadastro</p>
                    <p className="text-base font-medium">{formatarData(dados.dataCadastro)}</p>
                </div>

                <div>
                    <p className="text-sm text-gray-500">Status</p>
                    <p className="text-base font-medium">{dados.ativo ? 'Ativa' : 'Inativa'}</p>
                </div>
            </div>

            {/* Conteúdo da Agroindústria */}
            {dados.agroindustria && (
                <div className="bg-white shadow-lg rounded-lg p-8 grid grid-cols-1 md:grid-cols-2 gap-y-6 gap-x-16 text-green-dark">
                    <h2 className="text-xl font-bold col-span-full mb-4">Agroindústria</h2>

                    <div>
                        <p className="text-sm text-gray-500">Nome Fantasia</p>
                        <p className="text-base font-medium">{dados.agroindustria.nomeFantasia}</p>
                    </div>

                    <div>
                        <p className="text-sm text-gray-500">Razão Social</p>
                        <p className="text-base font-medium">{dados.agroindustria.razaoSocial}</p>
                    </div>

                    <div>
                        <p className="text-sm text-gray-500">CNPJ</p>
                        <p className="text-base font-medium">{dados.agroindustria.cnpj}</p>
                    </div>

                    <div>
                        <p className="text-sm text-gray-500">E-mail</p>
                        <p className="text-base font-medium">{dados.agroindustria.email}</p>
                    </div>

                    <div>
                        <p className="text-sm text-gray-500">Telefone</p>
                        <p className="text-base font-medium">{formatarTelefone(dados.agroindustria.telefone)}</p>
                    </div>

                    <div>
                        <p className="text-sm text-gray-500">Endereço</p>
                        <p className="text-base font-medium">{dados.agroindustria.endereco}</p>
                    </div>

                    <div>
                        <p className="text-sm text-gray-500">Data de Cadastro</p>
                        <p className="text-base font-medium">{formatarData(dados.agroindustria.dataCadastro)}</p>
                    </div>

                    <div>
                        <p className="text-sm text-gray-500">Status</p>
                        <p className="text-base font-medium">{dados.agroindustria.ativo ? 'Ativa' : 'Inativa'}</p>
                    </div>
                </div>
            )}
        </div>
    );
};

export default TemplateDetalhesGranja;
