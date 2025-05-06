import React from 'react';
import FormEdicaoPerfil from '../organisms/FormEdicaoPerfil';

const TemplatePerfil = ({ onSubmit, erros, initialData }) => {
    return (
        <div className="container mx-auto pt-8 h-fit">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-green-dark">Edição de Perfil</h1>
            </div>

            <div className="bg-white shadow-lg rounded-xl w-1/2 mx-auto py-12 mb-12 px-16 flex flex-col items-start">
                <FormEdicaoPerfil
                    onSubmit={onSubmit}
                    erros={erros}
                    initialData={initialData}
                />
            </div>
        </div>
    );
};

export default TemplatePerfil;
