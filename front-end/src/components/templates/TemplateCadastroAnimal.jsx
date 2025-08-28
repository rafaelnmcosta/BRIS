import React from 'react';
import FormCadastroAnimal from '../organisms/FormCadastroAnimal';

const TemplateCadastroAnimal = ({
    onSubmit,
    erros,
    carregarAgroindustrias,
    carregarGranjas,
    carregarUsuarios
}) => {
    return (
        <div className="container mx-auto pt-8 h-fit">
            {/* Header */}
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-green-dark">Cadastro de Animal</h1>
            </div>

            {/* Form */}
            <div className="bg-white shadow-lg rounded-xl w-1/2 mx-auto py-12 mb-12 px-16 flex flex-col items-start">
                <FormCadastroAnimal
                    onSubmit={onSubmit}
                    erros={erros}
                    carregarAgroindustrias={carregarAgroindustrias}
                    carregarGranjas={carregarGranjas}
                    carregarUsuarios={carregarUsuarios}
                />
            </div>
        </div>
    );
};

export default TemplateCadastroAnimal;
