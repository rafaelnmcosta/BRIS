import React from 'react';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { PlusOutlined } from '@ant-design/icons';

const FormAtivarUsuario = ({ onSubmit, novosVinculos, erros, onAbrirModal }) => {
    const handleSubmit = (e) => {
        e.preventDefault();

        if (novosVinculos.length === 0) {
            return;
        }

        onSubmit(novosVinculos);
    };

    return (
        <form className="w-full" onSubmit={handleSubmit}>
            {/* Seção de Vínculos */}
            <div className="my-6">
                <div className="flex justify-between items-center mb-4">
                    <h3 className="text-lg font-semibold">Vínculos</h3>
                    <button
                        type="button"
                        onClick={onAbrirModal}
                        className="flex items-center gap-2 text-green-dark hover:text-green"
                    >
                        <PlusOutlined /> Novo Vínculo
                    </button>
                </div>

                {/* Listagem de vínculos */}
                {novosVinculos.length > 0 ? (
                    novosVinculos.map((vinculo, index) => (
                        <div key={index} className="p-3 mb-2 border rounded-lg">
                            <p>Perfil: {vinculo.roleId}</p>
                            {vinculo.granjaId && <p>Granja: {vinculo.granjaId}</p>}
                            {vinculo.agroindustriaId && <p>Agroindústria: {vinculo.agroindustriaId}</p>}
                        </div>
                    ))
                ) : (
                    <p className="text-gray-500">Nenhum vínculo adicionado ainda.</p>
                )}

                {erros.novosVinculos && <p className="text-red-500 text-sm">{erros.novosVinculos}</p>}
            </div>

            <div className="w-1/2 mx-auto">
                <BotaoPrimario texto="Ativar Usuário" type="submit" />
            </div>
        </form>
    );
};

export default FormAtivarUsuario;
