import React from 'react';
import FormEdicaoUsuario from '../organisms/FormEdicaoUsuario';
import ModalVinculos from '../organisms/ModalVinculos';

const TemplateEdicaoUsuario = ({ onSubmit, initialData, vinculos, onAdicionarVinculo, onRemoverVinculo }) => {
    const [showModal, setShowModal] = React.useState(false);
    return (
        <div className="container mx-auto pt-8 h-fit">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-green-dark">Edição de usuário</h1>
            </div>
            
            <div className="bg-white shadow-lg rounded-xl w-1/2 mx-auto py-12 mb-12 px-16 flex flex-col items-start">
                <FormEdicaoUsuario
                    onSubmit={onSubmit}
                    initialData={initialData}
                    vinculos={vinculos}
                    onAbrirModal={() => setShowModal(true)}
                    onRemoverVinculo={onRemoverVinculo}
                />

                <ModalVinculos
                    visible={showModal}
                    onCancelar={() => setShowModal(false)}
                    onSalvar={(novoVinculo) => {
                        onAdicionarVinculo(novoVinculo);
                        setShowModal(false);
                    }}
                />
            </div>
        </div>
    );
};

export default TemplateEdicaoUsuario;