import React from 'react';
import FormCadastroUsuario from '../organisms/FormCadastroUsuario';
import ModalVinculos from '../organisms/ModalVinculos';

const TemplateCadastroUsuario = ({ onSubmit, erros, vinculos, onAdicionarVinculo }) => {
    const [showModal, setShowModal] = React.useState(false);

    return (
        <div className="container mx-auto pt-8 h-fit">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-green-dark">Cadastro de usuário</h1>
            </div>
            <div className="bg-white shadow-lg rounded-xl w-1/2 mx-auto py-12 mb-12 px-16 flex flex-col items-start">
                <FormCadastroUsuario
                    onSubmit={onSubmit}
                    erros={erros}
                    vinculos={vinculos}
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

export default TemplateCadastroUsuario;