import React from 'react';
import FormAtivarUsuario from '../organisms/FormAtivarUsuario';
import ModalVinculos from '../organisms/ModalVinculos';

const TemplateAtivarUsuario = ({ onSubmit, erros, novosVinculos, onAdicionarVinculo }) => {
    const [showModal, setShowModal] = React.useState(false);

    return (
        <div className="container mx-auto pt-8 h-fit">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-green-dark">Ativar usuário</h1>
            </div>

            <div className="bg-white shadow-lg rounded-xl w-1/2 mx-auto py-12 mb-12 px-16 flex flex-col items-start">
                <FormAtivarUsuario
                    onSubmit={onSubmit}
                    erros={erros}
                    novosVinculos={novosVinculos}
                    onAbrirModal={() => setShowModal(true)}
                />

                <ModalVinculos
                    visible={showModal}
                    onCancelar={() => setShowModal(false)}
                    onSalvar={onAdicionarVinculo}
                />
            </div>
        </div>
    );
};

export default TemplateAtivarUsuario;